using CamelsClub.API.Filters;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Services;
using CamelsClub.ViewModels;
using System.Web.Http;

namespace CamelsClub.API.Controllers.Comment
{
    public class CommentUserActionController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly ICommentUserActionService _commentUserActionService;

        public CommentUserActionController(IUnitOfWork unit, ICommentUserActionService postUserActionService)
        {
            _unit = unit;
            _commentUserActionService = postUserActionService;

        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/CommentUserComment/GetList")]
        public ResultViewModel<PagingViewModel<CommentUserActionViewModel>> GetList(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<CommentUserActionViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<CommentUserActionViewModel>>();
            resultViewModel.Data = _commentUserActionService.Search(orderBy, isAscending, pageIndex, pageSize, Language);
            resultViewModel.Success = true;
            resultViewModel.Message = Resource.DataLoaded;
            return resultViewModel;
        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/CommentUserAction/Add")]
        [ValidateViewModel]
        public ResultViewModel<bool> Add(CommentUserActionCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();

            var userID = int.Parse(UserID);
            if (userID != 0)
            {
                viewModel.UserID = userID;
                _commentUserActionService.Add(viewModel);
                _unit.Save();
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.CommentAddedSuccessfully;
            }
            else
            {
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.UserNotFound;
            }

            return resultViewModel;
        }

        [AuthorizeUserFilter(Role = "User")]
        [Route("api/CommentUserAction/Edit")]
        [ValidateViewModel]
        public ResultViewModel<bool> Edit(CommentUserActionCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();

            var userID = int.Parse(UserID);
            if (userID != 0)
            {
                viewModel.UserID = userID;
                _commentUserActionService.Edit(viewModel);
                _unit.Save();
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.PostAddedSuccessfully;
            }
            else
            {
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.UserNotFound;
            }

            return resultViewModel;
        }
        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/CommentUserAction/GetByID")]
        public ResultViewModel<CommentUserActionViewModel> GetByID(int id)
        {
            ResultViewModel<CommentUserActionViewModel> resultViewModel = new ResultViewModel<CommentUserActionViewModel>();

            if (_commentUserActionService.IsExists(id))
            {
                var res = _commentUserActionService.GetByID(id);
                resultViewModel.Success = true;
                resultViewModel.Data = res;
                resultViewModel.Message = Resource.DataLoaded;

            }
            else
            {
                resultViewModel.Success = false;
                resultViewModel.Message = Resource.CommentNotFound;
            }
            return resultViewModel;
        }
    }

       
}
