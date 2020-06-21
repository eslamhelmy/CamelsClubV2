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
    public class PageController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly IPageService _pageService;

        public PageController(IUnitOfWork unit, IPageService pageService)
        {
            _unit = unit;
            _pageService = pageService;

        }

        [HttpGet]
        [AuthorizeUserFilter(Role ="Admin")]

        [Route("api/Page/GetList")]
        public ResultViewModel<List<PageViewModel>> GetList()
        {
            try
            {
                ResultViewModel<List<PageViewModel>> resultViewModel = new ResultViewModel<List<PageViewModel>>();

                resultViewModel.Data = _pageService.GetList();
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;

            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<PageViewModel>>(ex.Message);
            }
        }

    }

       
}
