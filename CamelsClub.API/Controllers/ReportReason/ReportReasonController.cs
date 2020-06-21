using CamelsClub.API.Filters;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Services;
using CamelsClub.ViewModels;
using System.Web.Http;

namespace CamelsClub.API.Controllers
{
    public class ReportReasonController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly IReportReasonService _ReportReasonService;

        public ReportReasonController(IUnitOfWork unit, IReportReasonService ReportReasonService)
        {
            _unit = unit;
            _ReportReasonService = ReportReasonService;

        }

        [HttpGet]
        [AuthorizeUserFilter(Role ="User")]

        [Route("api/ReportReason/GetList")]
        public ResultViewModel<PagingViewModel<ReportReasonViewModel>> GetList(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<ReportReasonViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<ReportReasonViewModel>>();

            resultViewModel.Data = _ReportReasonService.Search(orderBy, isAscending, pageIndex, pageSize, Language);
            resultViewModel.Success = true;
            resultViewModel.Message = Resource.DataLoaded;
            return resultViewModel;
        }

        [HttpPost]
     //  [AuthorizeUserFilter(Role = "User")]
        [Route("api/ReportReason/Add")]
        [ValidateViewModel]
        public ResultViewModel<bool> Add(ReportReasonCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();

            //var userID = int.Parse(UserID);
            //if (userID != 0)
            //{
            //    viewModel.UserID = userID;
                _ReportReasonService.Add(viewModel);
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
        [Route("api/ReportReason/Edit")]
        [ValidateViewModel]
        public ResultViewModel<bool> Edit(ReportReasonCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();

            //var userID = int.Parse(UserID);
            //if (userID != 0)
            //{
            //    viewModel.UserID = userID;
                _ReportReasonService.Edit(viewModel);
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
        [Route("api/ReportReason/GetByID")]
        public ResultViewModel<ReportReasonViewModel> GetByID(int id)
        {
            ResultViewModel<ReportReasonViewModel> resultViewModel = new ResultViewModel<ReportReasonViewModel>();

            if (_ReportReasonService.IsExists(id))
            {
                var res = _ReportReasonService.GetByID(id);
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
