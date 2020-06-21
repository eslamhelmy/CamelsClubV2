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
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IUnitOfWork _unit;
        private readonly IFriendRequestRepository _repo;
        private readonly IFriendRepository _friendRepository;
        private readonly INotificationService _notificationService;

        public FriendRequestService(IUnitOfWork unit,
                                    IFriendRequestRepository repo ,
                                    IFriendRepository friendRepository,
                                    INotificationService notificationService)
        {
            _unit = unit;
            _repo = repo;
            _friendRepository = friendRepository;
                  _notificationService = notificationService;
        }
        // get my receivedfriendRequests
        public PagingViewModel<FriendRequestViewModel> Search(int userID , string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll()
                .Where(x => !x.IsDeleted)
                .Where(x=> x.ToUserID == userID)
                .Where(x => x.Status == (int)FriendRequestStatus.Pending);




            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<FriendRequestViewModel> result = new List<FriendRequestViewModel>();

            var requests = query.Select(obj => new FriendRequestViewModel
            {
                ID = obj.ID,
                ToUserMainImagePath = protocol + "://" + hostName + "/uploads/User-Document/" + obj.ToUser.UserProfile.MainImage,
                ToUserName = obj.ToUser.UserName,
                Notes = obj.Notes ,
                Status = obj.Status
            }).OrderByPropertyName(orderBy, isAscending);

            result = requests.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<FriendRequestViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }

        public PagingViewModel<SentFriendRequestViewModel> GetSentFriendRequests(int userID = 0, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll()
                .Where(x => !x.IsDeleted)
                .Where(x => x.FromUser.ID == userID)
                .Where(x=> x.Status == (int)FriendRequestStatus.Pending);
            



            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<SentFriendRequestViewModel> result = new List<SentFriendRequestViewModel>();

            var requests = query.Select(obj => new SentFriendRequestViewModel
            {
                ID = obj.ID,
                ToUserMainImagePath = protocol + "://" + hostName + "/uploads/User-Document/" + obj.ToUser.UserProfile.MainImage,
                ToUserName = obj.ToUser.UserName,
                ToUserID = obj.ToUserID,
                Notes = obj.Notes,
                Status = obj.Status
            }).OrderByPropertyName(orderBy, isAscending);

            result = requests.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<SentFriendRequestViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }

        public PagingViewModel<ReceivedFriendRequestViewModel> GetReceivedFriendRequests(int userID = 0, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll()
                .Where(x => !x.IsDeleted)
                .Where(x => x.ToUser.ID == userID)
                .Where(x=> x.Status == (int)FriendRequestStatus.Pending);



            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<ReceivedFriendRequestViewModel> result = new List<ReceivedFriendRequestViewModel>();

            var requests = query.Select(obj => new ReceivedFriendRequestViewModel
            {
                ID = obj.ID,
                FromUserMainImagePath = protocol + "://" + hostName + "/uploads/User-Document/" + obj.FromUser.UserProfile.MainImage,
                FromUserName = obj.FromUser.UserName,
                FromDisplayName = obj.FromUser.DisplayName,
                FromUserID = obj.FromUserID,
                Notes = obj.Notes,
                Status = obj.Status
            }).OrderByPropertyName(orderBy, isAscending);

            result = requests.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<ReceivedFriendRequestViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }

        public bool ApproveFriendRequest(FriendCreateViewModel viewModel)
        {
            //mark the request as Approved

         var request =
            _repo.GetAll().
                Where(x => x.FromUserID == viewModel.FriendUserID &&
                          x.ToUserID == viewModel.UserID &&
                          !x.IsDeleted && !(x.Status == (int)FriendRequestStatus.Approved))
                .FirstOrDefault();
            request.Status = (int)FriendRequestStatus.Approved;

            //add the FriendUserID to UserID as Friend
            _friendRepository.Add(new Friend
            {
                UserID = viewModel.UserID,
                FriendUserID = viewModel.FriendUserID
            });

            _unit.Save();

            var notifcation = new NotificationCreateViewModel
            {
                ContentArabic = "تم الموافقة علي طلب الصداقة",
                ContentEnglish = "The friend request was approved",
                NotificationTypeID = 5,
                SourceID = viewModel.UserID,
                DestinationID = viewModel.FriendUserID,
                FriendRequestID = request.ID

            };

             _notificationService.SendNotifictionForUser(notifcation);

            return true;
        }

        public bool IgnoreFriendRequest(FriendCreateViewModel viewModel)
        {
            //mark the request as Ignored

            var request =
               _repo.GetAll().
                   Where(x => x.FromUserID == viewModel.FriendUserID &&
                             x.ToUserID == viewModel.UserID &&
                             !x.IsDeleted && (x.Status == (int)FriendRequestStatus.Pending))
                   .FirstOrDefault();
            request.Status = (int)FriendRequestStatus.Ignored;
            
            _unit.Save();

            //var notifcation = new NotificationCreateViewModel
            //{
            //    ContentArabic = "تم رفض طلب الصداقة ",
            //    ContentEnglish = "a friend request was rejected",
            //    NotificationTypeID = 13,
            //    SourceID = viewModel.UserID,
            //    DestinationID = viewModel.FriendUserID,
            //    FriendRequestID = request.ID

            //};

            //_notificationService.SendNotifictionForUser(notifcation);

            return true;
        }
        public void Add(FriendRequestCreateViewModel viewModel)
        {
            if(!_repo
                .GetAll()
                .Where(x=>((x.FromUserID == viewModel.FromUserID || x.FromUserID == viewModel.ToUserID) && x.Status == (int)FriendRequestStatus.Pending) && ( (x.ToUserID == viewModel.ToUserID || x.ToUserID ==viewModel.FromUserID) && x.Status == (int)FriendRequestStatus.Pending ) && !x.IsDeleted)
                .Any())
            {
              var insertedReq=  _repo.Add(viewModel.ToModel());
                _unit.Save();
                var notifcation = new NotificationCreateViewModel
                {
                    ContentArabic = "تم إستلام طلب صداقة ",
                    ContentEnglish = "a friend request was received",
                    NotificationTypeID = 4,
                    SourceID = viewModel.FromUserID,
                    DestinationID = viewModel.ToUserID,
                    FriendRequestID = insertedReq.ID

                };
                _notificationService.SendNotifictionForUser(notifcation);
            }
            else
            {
                throw new Exception("You can not send friend request because you have request from this user");
            }
         
        }

        public void Edit(FriendRequestCreateViewModel viewModel)
        {
            _repo.SaveIncluded(viewModel.ToModel(), "Status");           
        }
        public FriendRequestViewModel GetByID(int id)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var postUserAction = _repo.GetAll().Where(postAction => postAction.ID == id)
                .Select(obj => new FriendRequestViewModel
            {
                    ID = obj.ID,
                    ToUserMainImagePath = protocol + "://" + hostName + "/uploads/User-Document/" + obj.ToUser.UserProfile.MainImage,
                    ToUserName = obj.ToUser.UserName,
                    Notes = obj.Notes,
                    Status = obj.Status
                }).FirstOrDefault();
            
            return postUserAction;

        }

        public bool IsExists(int id)
        {
            return _repo.GetAll().Where(x => x.ID == id && !x.IsDeleted).Any();
        }
        public void Delete(int id)
        {
             _repo.Remove(id);
        }
    }

    public class SentFriendRequestViewModel
    {
        public int ID { get; set; }
        public string ToUserMainImagePath { get; set; }
        public string ToUserName { get; set; }
        public int ToUserID { get; set; }
        public string Notes { get; set; }
        public int Status { get; set; }
    }
}

