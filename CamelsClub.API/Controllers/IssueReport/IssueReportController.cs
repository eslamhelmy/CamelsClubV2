using CamelsClub.API.Filters;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Services;
using CamelsClub.ViewModels;
using System.Web.Http;

namespace CamelsClub.API.Controllers
{
    public class IssueReportController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly IIssueReportService _issueReportService;

        public IssueReportController(IUnitOfWork unit, IIssueReportService issueReportService)
        {
            _unit = unit;
            _issueReportService = issueReportService;

        }

        [HttpGet]
        [AuthorizeUserFilter(Role ="User")]
        [Route("api/IssueReport/GetList")]
        public ResultViewModel<PagingViewModel<IssueReportViewModel>> GetList(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<IssueReportViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<IssueReportViewModel>>();

            resultViewModel.Data = _issueReportService.Search(orderBy, isAscending, pageIndex, pageSize, Language);
            resultViewModel.Success = true;
            resultViewModel.Message = Resource.DataLoaded;
            return resultViewModel;
        }

        [HttpPost]
       [AuthorizeUserFilter(Role = "User")]
        [Route("api/IssueReport/Add")]
        [ValidateViewModel]
        public ResultViewModel<bool> Add(IssueReportCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
            viewModel.UserID = int.Parse(UserID);
            if (!_issueReportService.IsExists(viewModel.PostID, viewModel.UserID))
            {
                _issueReportService.Add(viewModel);
                _unit.Save();

            }
            resultViewModel.Success = true;
                resultViewModel.Data = true;
            resultViewModel.Message = Resource.AddedSuccessfully;
            
            return resultViewModel;
        }

        [AuthorizeUserFilter(Role = "User")]
        [Route("api/IssueReport/Edit")]
        [ValidateViewModel]
        public ResultViewModel<bool> Edit(IssueReportCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
            
                viewModel.UserID = int.Parse(UserID);
            _issueReportService.Edit(viewModel);
                _unit.Save();
                resultViewModel.Success = true;
                resultViewModel.Data = true;

                resultViewModel.Message = Resource.PostAddedSuccessfully;
            
            return resultViewModel;
        }
        [HttpGet]
      //  [AuthorizeUserFilter(Role = "User")]
        [Route("api/IssueReport/GetByID")]
        public ResultViewModel<IssueReportViewModel> GetByID(int id)
        {
            ResultViewModel<IssueReportViewModel> resultViewModel = new ResultViewModel<IssueReportViewModel>();

           // if (_issueReportService.IsExists(id))
           // {
                var res = _issueReportService.GetByID(id);
                resultViewModel.Success = true;
                resultViewModel.Data = res;
                resultViewModel.Message = Resource.DataLoaded;

          //  }
          //  else
          //  {
            //    resultViewModel.Success = false;
              //  resultViewModel.Message = Resource.CommentNotFound;
         //   }
            return resultViewModel;
        }
    }

       
}
