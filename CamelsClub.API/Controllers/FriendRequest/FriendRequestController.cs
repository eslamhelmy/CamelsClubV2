using CamelsClub.API.Filters;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Services;
using CamelsClub.ViewModels;
using System;
using System.Web.Http;

namespace CamelsClub.API.Controllers
{
    public class FriendRequestController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly IFriendRequestService _friendRequestService;

        public FriendRequestController(IUnitOfWork unit, IFriendRequestService friendRequestService)
        {
            _unit = unit;
            _friendRequestService = friendRequestService;

        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/FriendRequest/ApproveFriendRequest")]
        public ResultViewModel<bool> ApproveFriendRequest(FriendCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
                viewModel.UserID= int.Parse(UserID);
                resultViewModel.Data = _friendRequestService.ApproveFriendRequest(viewModel);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.FriendRequestApporoved;
                return resultViewModel;
         
        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/FriendRequest/IgnoreFriendRequest")]
        public ResultViewModel<bool> IgnoreFriendRequest(FriendCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
                viewModel.UserID = int.Parse(UserID);
                resultViewModel.Data = _friendRequestService.IgnoreFriendRequest(viewModel);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.FriendRequestApporoved;
                return resultViewModel;
          
        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/FriendRequest/GetSentFriendRequests")]
        public ResultViewModel<PagingViewModel<SentFriendRequestViewModel>> GetSentFriendRequests(int userID = 0, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<SentFriendRequestViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<SentFriendRequestViewModel>>();
            try
            {
                var loggedUserID = int.Parse(UserID);
                resultViewModel.Data = _friendRequestService.GetSentFriendRequests(loggedUserID, orderBy, isAscending, pageIndex, pageSize, Language);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;
            }
            catch (System.Exception ex)
            {
                return new ResultViewModel<PagingViewModel<SentFriendRequestViewModel>>(null, ex.Message, false);
            }

        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/FriendRequest/GetReceivedFriendRequests")]
        public ResultViewModel<PagingViewModel<ReceivedFriendRequestViewModel>> GetReceivedFriendRequests(int userID = 0, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<ReceivedFriendRequestViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<ReceivedFriendRequestViewModel>>();
            try
            {
                var loggedUserID = int.Parse(UserID);
                resultViewModel.Data = _friendRequestService.GetReceivedFriendRequests(loggedUserID, orderBy, isAscending, pageIndex, pageSize, Language);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;
            }
            catch (System.Exception ex)
            {
                return new ResultViewModel<PagingViewModel<ReceivedFriendRequestViewModel>>(null, ex.Message, false);
            }

        }

        [HttpGet]
       [AuthorizeUserFilter(Role ="User")]
        [Route("api/FriendRequest/GetList")]
        public ResultViewModel<PagingViewModel<FriendRequestViewModel>> GetList(int postID = 0,string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<FriendRequestViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<FriendRequestViewModel>>();
                resultViewModel.Data = _friendRequestService.Search(postID, orderBy, isAscending, pageIndex, pageSize, Language);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;
           
        }
        
        [HttpPost]
       [AuthorizeUserFilter(Role = "User")]
        [Route("api/FriendRequest/Add")]
        [ValidateViewModel]
        public ResultViewModel<bool> Add(FriendRequestCreateViewModel viewModel)
        {
            try
            {
                ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();

                viewModel.FromUserID = int.Parse(UserID);
                _friendRequestService.Add(viewModel);
                resultViewModel.Data = true;
                _unit.Save();
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.AddedSuccessfully;
                return resultViewModel;

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }
           
        }

        [AuthorizeUserFilter(Role = "User")]
        [Route("api/FriendRequest/Edit")]
        [ValidateViewModel]
        public ResultViewModel<bool> Edit(FriendRequestCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
                viewModel.FromUserID = int.Parse(UserID);
                _friendRequestService.Edit(viewModel);
                _unit.Save();
                resultViewModel.Success = true;
                resultViewModel.Data = true;
                resultViewModel.Message = Resource.PostAddedSuccessfully;
                return resultViewModel;
           }
        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/FriendRequest/GetByID")]
        public ResultViewModel<FriendRequestViewModel> GetByID(int id)
        {
            ResultViewModel<FriendRequestViewModel> resultViewModel = new ResultViewModel<FriendRequestViewModel>();
                var res = _friendRequestService.GetByID(id);
                resultViewModel.Success = true;
                resultViewModel.Data = res;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;
                
        }
    }

       
}
