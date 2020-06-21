using CamelsClub.Data.Extentions;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Models;
using CamelsClub.Repositories;
using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CamelsClub.Services
{
    public class FriendService : IFriendService
    {
        private readonly IUnitOfWork _unit;
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IFriendRepository _friendRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBlockedFriendRepository _blockedFriendRepository;

        public FriendService(IUnitOfWork unit,
                                    IFriendRequestRepository friendRequestRepository ,
                                    IUserRepository userRepository,
                                    IBlockedFriendRepository blockedFriendRepository ,
                                    IFriendRepository friendRepository )
        {
            _unit = unit;
            _userRepository = userRepository;
            _friendRequestRepository = friendRequestRepository;
            _friendRepository = friendRepository;
            _blockedFriendRepository = blockedFriendRepository;
        }
        // get my friends
        public PagingViewModel<ClearedFriendViewModel> Search(int userID=0 , string search="", string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            //get blocked friends 
            //var friendsIBlocked = _userRepository.GetAll().Where(x => x.ID == userID).SelectMany(x => x.BlockedFriends).Select(b => b.UserID).ToList();
            //var friendsBlockedMe = _userRepository.GetAll().Where(x => x.ID == userID).SelectMany(x => x.FriendsBlockedMe).Select(b => b.UserID).ToList();
            //var allBlocked = new List<int>();
            //allBlocked.AddRange(friendsIBlocked);
            //allBlocked.AddRange(friendsBlockedMe);
            var query = _friendRepository.GetAll()
                .Where(x => !x.IsBlocked)
                .Where(x => x.UserID == userID || x.FriendUserID == userID)
                .Where(x => search == "" || x.FriendUser.UserName.Contains(search));
            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<FriendViewModel> result = new List<FriendViewModel>();

            var requests = query.Select(obj => new FriendViewModel
            {
                ID = obj.ID,
                UserName = obj.User.UserName,
                DisplayName = obj.User.DisplayName,
                UserID = obj.UserID,
                UserMainImagePath = protocol + "://" + hostName + "/uploads/User-Document/" + obj.User.UserProfile.MainImage,
                FriendMainImagePath = protocol + "://" + hostName + "/uploads/User-Document/" + obj.FriendUser.UserProfile.MainImage,
                FriendUserName = obj.FriendUser.UserName,
                FriendUserID = obj.FriendUserID 
             
            }).OrderByDescending(x=>x.ID);

            result = requests.Skip(excludedRows).Take(pageSize).ToList();
            //return final list
            List<ClearedFriendViewModel> list = new List<ClearedFriendViewModel>();
            foreach (var item in result)
            {
                if(userID == item.UserID)
                {
                    list.Add(new ClearedFriendViewModel
                    {
                        ID = item.ID,
                        UserID = item.FriendUserID,
                        UserName = item.FriendUserName,
                        DisplayName = item.DisplayName,
                        MainImagePath = item.FriendMainImagePath
                    });
                }else if(userID == item.FriendUserID)
                {
                    list.Add(new ClearedFriendViewModel
                    {
                        ID = item.ID,
                        UserID = item.UserID,
                        UserName = item.UserName,
                        DisplayName = item.DisplayName,
                        MainImagePath = item.UserMainImagePath
                    });
                }
            }
            return new PagingViewModel<ClearedFriendViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = list, Records = records, Pages = pages };
        }
        
        public FriendViewModel GetByID(int id)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var friend = _friendRepository.GetAll().Where(x => x.ID == id)
                .Select(obj => new FriendViewModel
            {
                    ID = obj.ID,
                    FriendMainImagePath = protocol + "://" + hostName + "/uploads/User-Document/" + obj.FriendUser.UserProfile.MainImage,
                    FriendUserName = obj.FriendUser.UserName,
                    FriendUserID = obj.FriendUserID
                }).FirstOrDefault();
            
            return friend;

        }
        
        public void Delete(int id)
        {
             _friendRepository.Remove(id);
        }

        public bool Unfollow(BlockedFriendCreateViewModel viewModel)
        {
            var friend = _friendRepository.GetAll()
                .Where(x => (x.FriendUserID == viewModel.BlockedFriendID && x.UserID == viewModel.UserID) || (x.FriendUserID == viewModel.UserID && x.UserID == viewModel.BlockedFriendID)).FirstOrDefault();
            if(friend != null)
            {
                friend.IsBlocked = true;
                _unit.Save();
            }
        //    _blockedFriendRepository.Add(viewModel.ToModel());
            return true;
        }
        public bool IsAlreadyBlocked(int userID, int blockedFriendID)
        {
           return
            _blockedFriendRepository.GetAll()
                .Where(x => x.BlockedFriendID == blockedFriendID && x.UserID == userID)
                .Any();
        }

    }


}

