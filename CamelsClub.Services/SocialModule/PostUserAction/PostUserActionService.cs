using CamelsClub.Data.Extentions;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Models;
using CamelsClub.Models.Enums;
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
    public class PostUserActionService : IPostUserActionService
    {
        private readonly IUnitOfWork _unit;
        private readonly IPostUserActionRepository _repo;
        private readonly INotificationService _notificationService;
        private readonly IFriendRepository _friendRepository;
        public PostUserActionService(IUnitOfWork unit, IPostUserActionRepository repo, INotificationService notificationService, IFriendRepository friendRepository)
        {
            _unit = unit;
            _repo = repo;
            _notificationService = notificationService;
            _friendRepository = friendRepository;
        }
        public PagingViewModel<PostUserActionViewModel> Search(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll().
                            Where(x => !x.IsDeleted).
                            Where(x => x.IsActive);
                            



            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<PostUserActionViewModel> result = new List<PostUserActionViewModel>();

            var posts = query.Select(obj => new PostUserActionViewModel
            {
                ID = obj.ID,
                ActionID = obj.ActionID,
                PostID = obj.PostID ,
                UserID = obj.UserID
            }).OrderByPropertyName(orderBy, isAscending);

            result = posts.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<PostUserActionViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }


        //it acts as Delete , but in new form
        public bool Add(PostUserActionCreateViewModel viewModel)
        {
            PostUserAction res;
            var postUserAction =
                _repo.GetAll().FirstOrDefault(x => !x.IsDeleted &&
                                    x.PostID == viewModel.PostID &&
                                    x.UserID == viewModel.UserID &&
                                    x.ActionID == (int)Actions.Like
                                    );
            if(postUserAction != null) 
            {
                res= _repo.SaveIncluded(new PostUserAction
                {
                    ID = postUserAction.ID,
                    IsActive = !postUserAction.IsActive
                }, "IsActive");

                _unit.Save();

                return res.IsActive;
            }
            else
            {
                res=_repo.Add(viewModel.ToModel());
                _unit.Save();
                var usersIDs = _friendRepository.FriendUsersIDs(viewModel.UserID);

                var notifcation = new NotificationCreateViewModel
                {
                    ContentArabic = "تم الاعجاب بالمنشور  ",
                    ContentEnglish = "Post has been liked",
                    NotificationTypeID = 7,
                    SourceID = viewModel.UserID,
                  //  DestinationID = res.Post.UserID,
                    PostID = viewModel.PostID,
                    ActionID=viewModel.ActionID

                };

                _notificationService.SendNotifictionForFriends(notifcation, usersIDs);

                //_notificationService.SendNotifictionForUser(notifcation);
                return true;
            }
            
        }

        public void Edit(PostUserActionCreateViewModel viewModel)
        {
            _repo.SaveIncluded(viewModel.ToModel(), "ActionID");
            
        }

        public PostUserActionViewModel GetByID(int id)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var postUserAction = _repo.GetAll().Where(postAction => postAction.ID == id)
                .Select(obj => new PostUserActionViewModel
            {
                ID = obj.ID,
                UserID = obj.UserID,
                PostID = obj.PostID,
                ActionID = obj.ActionID
            }).FirstOrDefault();
            
            return postUserAction;

        }


        public bool IsExists(int id)
        {
            return _repo.GetAll().Where(x => x.ID == id).Any();
        }


      }
}

