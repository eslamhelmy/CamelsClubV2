using CamelsClub.API.Filters;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Services;
using CamelsClub.ViewModels;
using System.Web.Http;

namespace CamelsClub.API.Controllers.Comment
{
    public class CategoryController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly ICategoryService _categoryService;

        public CategoryController(IUnitOfWork unit, ICategoryService categoryService)
        {
            _unit = unit;
            _categoryService = categoryService;

        }

        [HttpGet]
        [AuthorizeUserFilter(Role ="User")]

        [Route("api/Category/GetList")]
        public ResultViewModel<PagingViewModel<CategoryViewModel>> GetList(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<CategoryViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<CategoryViewModel>>();

            resultViewModel.Data = _categoryService.Search(orderBy, isAscending, pageIndex, pageSize, Language);
            resultViewModel.Success = true;
            resultViewModel.Message = Resource.DataLoaded;
            return resultViewModel;
        }

        [HttpPost]
     //  [AuthorizeUserFilter(Role = "User")]
        [Route("api/Category/Add")]
        [ValidateViewModel]
        public ResultViewModel<bool> Add(CategoryCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();

            //var userID = int.Parse(UserID);
            //if (userID != 0)
            //{
            //    viewModel.UserID = userID;
                _categoryService.Add(viewModel);
                _unit.Save();
                resultViewModel.Success = true;
                resultViewModel.Data = true;
            resultViewModel.Message = Resource.CommentAddedSuccessfully;
            //}
            //else
            //{
            //    resultViewModel.Success = true;
            //    resultViewModel.Message = Resource.UserNotFound;
            //}

            return resultViewModel;
        }

      //  [AuthorizeUserFilter(Role = "User")]
        [Route("api/Category/Edit")]
        [ValidateViewModel]
        public ResultViewModel<bool> Edit(CategoryCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();

            //var userID = int.Parse(UserID);
            //if (userID != 0)
            //{
            //    viewModel.UserID = userID;
                _categoryService.Edit(viewModel);
                _unit.Save();
                resultViewModel.Success = true;
                resultViewModel.Data = true;

                resultViewModel.Message = Resource.PostAddedSuccessfully;
            //}
            //else
            //{
            //    resultViewModel.Success = true;
            //    resultViewModel.Message = Resource.UserNotFound;
            //}

            return resultViewModel;
        }
        [HttpGet]
      //  [AuthorizeUserFilter(Role = "User")]
        [Route("api/Category/GetByID")]
        public ResultViewModel<CategoryViewModel> GetByID(int id)
        {
            ResultViewModel<CategoryViewModel> resultViewModel = new ResultViewModel<CategoryViewModel>();

            if (_categoryService.IsExists(id))
            {
                var res = _categoryService.GetByID(id);
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
