using CamelsClub.API.Filters;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Repositories;
using CamelsClub.Services;
using CamelsClub.ViewModels;
using System.Web.Http;

namespace CamelsClub.API.Controllers.Post
{
    public class PostUserActionController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly IPostUserActionService _postUserActionService;

        private readonly IFriendRepository _friendRepository;

        public PostUserActionController(IUnitOfWork unit, IPostUserActionService postUserActionService, IFriendRepository friendRepository)
        {
            _unit = unit;
            _postUserActionService = postUserActionService;
            _friendRepository = friendRepository;

        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/PostUserPost/GetList")]
        public ResultViewModel<PagingViewModel<PostUserActionViewModel>> GetList(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<PostUserActionViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<PostUserActionViewModel>>();

            resultViewModel.Data = _postUserActionService.Search(orderBy, isAscending, pageIndex, pageSize, Language);
            resultViewModel.Success = true;
            resultViewModel.Message = Resource.DataLoaded;
            return resultViewModel;
        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/PostUserAction/Add")]
        [ValidateViewModel]
        public ResultViewModel<bool> CreatePostUserAction(PostUserActionCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
          
                viewModel.UserID = int.Parse(UserID);
                var res = _postUserActionService.Add(viewModel);
                _unit.Save();
                resultViewModel.Success = true;
                resultViewModel.Data = res;
                resultViewModel.Message = Resource.AddedSuccessfully;


                return resultViewModel;

           }
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/PostUserAction/Edit")]
        [ValidateViewModel]
        public ResultViewModel<bool> Edit(PostUserActionCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();

            var userID = int.Parse(UserID);
            if (userID != 0)
            {
                viewModel.UserID = userID;
                _postUserActionService.Edit(viewModel);
                _unit.Save();
                resultViewModel.Success = true;
                resultViewModel.Data = true;
                resultViewModel.Message = Resource.PostAddedSuccessfully;
            }
            else
            {
                resultViewModel.Success = false;

                resultViewModel.Message = Resource.UserNotFound;
            }

            return resultViewModel;
        }
        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/PostUserAction/GetByID")]
        public ResultViewModel<PostUserActionViewModel> GetByID(int id)
        {
            ResultViewModel<PostUserActionViewModel> resultViewModel = new ResultViewModel<PostUserActionViewModel>();

            if (_postUserActionService.IsExists(id))
            {
                var res = _postUserActionService.GetByID(id);
                resultViewModel.Success = true;
                resultViewModel.Data = res;
                resultViewModel.Message = Resource.DataLoaded;

            }
            else
            {
                resultViewModel.Success = false;
                resultViewModel.Message = Resource.PostNotFound;
            }
            return resultViewModel;
        }
    }

       
}
