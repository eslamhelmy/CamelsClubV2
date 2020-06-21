using CamelsClub.API.Filters;
using CamelsClub.API.Helpers;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Services;
using CamelsClub.Services.Helpers;
using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CamelsClub.API.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

       
        [HttpGet]
        [AuthorizeUserFilter(Role = "Admin") ]
        [Route("api/Dashboard/GetData")]
        public ResultViewModel<DashboardViewModel> GetData()
        {
            try
            {
                ResultViewModel<DashboardViewModel> resultViewModel = new ResultViewModel<DashboardViewModel>();
                var userID = int.Parse(UserID);
                resultViewModel.Data = _dashboardService.GetDashboardData();
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;

            }
            catch (Exception ex)
            {
                return new ResultViewModel<DashboardViewModel>(ex.Message);
            }
        }

        }
}
