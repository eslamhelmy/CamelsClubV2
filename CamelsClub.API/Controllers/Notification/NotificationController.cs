using CamelsClub.API.Filters;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Services;
using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace CamelsClub.API.Controllers.Comment
{
    public class NotificationController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly INotificationService _NotificationService;

        public NotificationController(IUnitOfWork unit, INotificationService categoryService)
        {
            _unit = unit;
            _NotificationService = categoryService;

        }

        [HttpGet]
        [AuthorizeUserFilter(Role ="User")]
        [Route("api/Notification/GetMyNotifications")]
        public ResultViewModel<NotificationPagingViewModel<NotificationViewModel>> GetMyNotifications(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<NotificationPagingViewModel<NotificationViewModel>> resultViewModel = new ResultViewModel<NotificationPagingViewModel<NotificationViewModel>>();
                var userID = int.Parse(UserID);
                resultViewModel.Data = _NotificationService.GetMyNotifications(userID, orderBy, isAscending, pageIndex, pageSize, Language);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;
           }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Notification/GetUnSeenNotificationsCount")]
        public ResultViewModel<UnSeenNotificationViewModel> GetUnSeenNotificationsCount()
        {
            var resultViewModel = new ResultViewModel<UnSeenNotificationViewModel>();
            var userID = int.Parse(UserID);
            resultViewModel.Data = _NotificationService.GetUnSeenNotificationsAndMessagesCount(userID);
            resultViewModel.Success = true;
            resultViewModel.Message = Resource.DataLoaded;
            return resultViewModel;
        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Notification/GetTypes")]
        public ResultViewModel<List<NotificationTypeViewModel>> GetTypes()
        {
            ResultViewModel<List<NotificationTypeViewModel>> resultViewModel = new ResultViewModel<List<NotificationTypeViewModel>>();
            var userID = int.Parse(UserID);
            resultViewModel.Data = _NotificationService.GetTypes();
            resultViewModel.Success = true;
            resultViewModel.Message = Resource.DataLoaded;
            return resultViewModel;
        }



        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Notification/IsSeen")]
        public ResultViewModel<bool> MakeNotificationSeen(int notifcationID)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
         
                if (_NotificationService.IsExist(notifcationID))
                {
                    var userID = int.Parse(UserID);
                    _NotificationService.UpdateNotificationStatus(notifcationID);
                    resultViewModel.Data = true;
                    resultViewModel.Success = true;
                    resultViewModel.Message = Resource.DataLoaded;
                }
                else
                {
                    resultViewModel.Success = false;
                    resultViewModel.Message = Resource.NotFound;
                }
                return resultViewModel;
         }


    }

       
}
