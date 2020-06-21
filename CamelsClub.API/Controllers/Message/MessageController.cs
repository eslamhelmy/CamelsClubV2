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
    public class MessageController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly IMessageService _messageService;

        public MessageController(IUnitOfWork unit, IMessageService messageService)
        {
            _unit = unit;
            _messageService = messageService;

        }
        [HttpGet]
        [Route("api/GetCurrentDate")]
        public DateTime GetCurrentDate()
        {
            return DateTime.Now.ToLocalTime();
        }
        [HttpGet]
        [AuthorizeUserFilter(Role ="User")]
        [Route("api/User/GetReceivedMessage")]
        public ResultViewModel<PagingViewModel<MessageViewModel>> GetMessages(int FromUserID , string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<MessageViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<MessageViewModel>>();
                var loggedUserID = int.Parse(UserID);
                resultViewModel.Data = _messageService.GetReceivedMessage(FromUserID,loggedUserID, orderBy, isAscending, pageIndex, pageSize, Language);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;
          }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Chat/GetUsers")]
        public ResultViewModel<List<UserChatViewModel>> GetUsersHasChatWithLoggedUser()
        {
            ResultViewModel<List<UserChatViewModel>> resultViewModel = new ResultViewModel<List<UserChatViewModel>>();
                var loggedUserID = int.Parse(UserID);
                resultViewModel.Data = _messageService.GetUsersHasChatWithLoggedUser(loggedUserID);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;
           
        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Chat/GetUnSeenMessagesCount")]
        public ResultViewModel<int> GetUnSeenMessagesCount()
        {
            ResultViewModel<int> resultViewModel = new ResultViewModel<int>();
                var loggedUserID = int.Parse(UserID);
                resultViewModel.Data = _messageService.GetUnSeenMessagesCount(loggedUserID);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;
           
        }


        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]

        [Route("api/User/SendMessage")]
        public ResultViewModel<bool> SendMessage(MessageCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
                viewModel.FromUserID = int.Parse(UserID);
                resultViewModel.Data = _messageService.Send(viewModel);
                _unit.Save();
               
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.UpdatedSuccessfully;
                return resultViewModel;
         }

    }

       
}
