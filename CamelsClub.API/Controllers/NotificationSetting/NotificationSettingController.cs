using CamelsClub.API.Filters;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Services;
using CamelsClub.ViewModels;
using System.Collections.Generic;
using System.Web.Http;

namespace CamelsClub.API.Controllers
{
    public class NotificationSettingController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly IUserNotificationSettingService _userNotificationSettingService;

        public NotificationSettingController(IUnitOfWork unit, IUserNotificationSettingService userNotificationSettingService)
        {
            _unit = unit;
            _userNotificationSettingService = userNotificationSettingService;
        }

        [HttpGet]
        [AuthorizeUserFilter(Role ="User")]
        [Route("api/NotificationSettings")]
        public ResultViewModel<List<NotificationSettingEditViewModel>> GetNotificationSetting()
        {
            ResultViewModel<List<NotificationSettingEditViewModel>> resultViewModel = new ResultViewModel<List<NotificationSettingEditViewModel>>();

            var loggedUserID = int.Parse(UserID);
            resultViewModel.Data = _userNotificationSettingService.GetNotificationSetting(loggedUserID, Language);
            resultViewModel.Success = true;
            resultViewModel.Message = Resource.DataLoaded;
            return resultViewModel;
        }

        [HttpPut]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/NotificationSettings")]
        [ValidateViewModel]
        public ResultViewModel<bool> Edit(List<NotificationSettingEditViewModel> list)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();

            var loggedUserID = int.Parse(UserID);
            resultViewModel.Data = _userNotificationSettingService.EditNotificationSetting(loggedUserID,list);
            resultViewModel.Success = true;
            return resultViewModel;
        }

    }

       
}
