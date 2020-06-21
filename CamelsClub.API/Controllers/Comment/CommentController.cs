using CamelsClub.API.Filters;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Services;
using CamelsClub.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http;

namespace CamelsClub.API.Controllers.Comment
{
    public class CommentController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly ICommentService _commentService;

        public CommentController(IUnitOfWork unit, ICommentService categoryService)
        {
            _unit = unit;
            _commentService = categoryService;

        }

        [HttpGet]
        [AuthorizeUserFilter(Role ="User")]

        [Route("api/Comment/GetList")]
        public ResultViewModel<PagingViewModel<CommentViewModel>> GetList(int postID = 0,string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<CommentViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<CommentViewModel>>();
                resultViewModel.Data = _commentService.Search(postID, orderBy, isAscending, pageIndex, pageSize, Language);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;
           
           
        }
        [HttpGet]
         [AuthorizeUserFilter(Role ="User")]

        [Route("api/Comment/GetReplies")]
        public ResultViewModel<IEnumerable<CommentViewModel>> GetReplies(int commentID)
        {
            ResultViewModel<IEnumerable<CommentViewModel>> resultViewModel = new ResultViewModel<IEnumerable<CommentViewModel>>();
                resultViewModel.Data = _commentService.GetReplies(commentID);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;

        }
        [HttpPost]
       [AuthorizeUserFilter(Role = "User")]
        [Route("api/Comment/Add")]
        [ValidateViewModel]
        public ResultViewModel<bool> Add(CommentCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();

            var userID = int.Parse(UserID);
            if (userID != 0)
            {
                viewModel.UserID = userID;
                _commentService.Add(viewModel);
                _unit.Save();
                resultViewModel.Success = true;
                resultViewModel.Data = true;
            resultViewModel.Message = Resource.CommentAddedSuccessfully;
            }
            else
            {
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.UserNotFound;
            }

            return resultViewModel;
        }

      //  [AuthorizeUserFilter(Role = "User")]
        [Route("api/Comment/Edit")]
        [ValidateViewModel]
        public ResultViewModel<bool> Edit(CommentCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();

            var userID = int.Parse(UserID);
            if (userID != 0)
            {
                viewModel.UserID = userID;
                _commentService.Edit(viewModel);
                _unit.Save();
                resultViewModel.Success = true;
                resultViewModel.Data = true;

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
      //  [AuthorizeUserFilter(Role = "User")]
        [Route("api/Comment/GetByID")]
        public ResultViewModel<CommentViewModel> GetByID(int id)
        {
            ResultViewModel<CommentViewModel> resultViewModel = new ResultViewModel<CommentViewModel>();

            if (_commentService.IsExists(id))
            {
                var res = _commentService.GetByID(id);
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
