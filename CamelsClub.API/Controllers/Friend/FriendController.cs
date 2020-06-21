using CamelsClub.API.Filters;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Services;
using CamelsClub.ViewModels;
using System;
using System.Web.Http;

namespace CamelsClub.API.Controllers.Comment
{
    public class FriendController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly IFriendService _friendService;

        public FriendController(IUnitOfWork unit, IFriendService categoryService)
        {
            _unit = unit;
            _friendService = categoryService;

        }

        [HttpGet]
        [AuthorizeUserFilter(Role ="User")]

        [Route("api/Friend/GetList")]
        public ResultViewModel<PagingViewModel<ClearedFriendViewModel>> GetList(string searchKeyword="",string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<ClearedFriendViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<ClearedFriendViewModel>>();
                var userID = int.Parse(UserID);
                resultViewModel.Data = _friendService.Search(userID, searchKeyword, orderBy, isAscending, pageIndex, pageSize, Language);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;
            
        }


        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]

        [Route("api/Friend/UnFollow")]
        public ResultViewModel<bool> UnFollow(BlockedFriendCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
                viewModel.UserID = int.Parse(UserID);
             //   if (!_friendService.IsAlreadyBlocked(viewModel.UserID, viewModel.BlockedFriendID))
             //   {
                    resultViewModel.Data = _friendService.Unfollow(viewModel);
                    _unit.Save();
             //   }
                resultViewModel.Data = true;
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.UpdatedSuccessfully;
                return resultViewModel;
          
        }


        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Friend/GetByID")]
        public ResultViewModel<FriendViewModel> GetByID(int id)
        {
            ResultViewModel<FriendViewModel> resultViewModel = new ResultViewModel<FriendViewModel>();
                var res = _friendService.GetByID(id);
                resultViewModel.Success = true;
                resultViewModel.Data = res;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;

        }
    }

       
}
