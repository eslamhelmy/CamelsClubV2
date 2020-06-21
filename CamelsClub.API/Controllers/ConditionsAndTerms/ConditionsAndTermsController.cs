using CamelsClub.API.Filters;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Services;
using CamelsClub.ViewModels;
using System.Web.Http;

namespace CamelsClub.API.Controllers
{
    public class ConditionsAndTermsController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly IConditionsAndTermsService _ConditionsAndTermsService;

        public ConditionsAndTermsController(IUnitOfWork unit, IConditionsAndTermsService ConditionsAndTermsService)
        {
            _unit = unit;
            _ConditionsAndTermsService = ConditionsAndTermsService;

        }

        [HttpGet]
        [Route("api/ConditionsAndTerms/Get")]
        public ResultViewModel<ConditionsAndTermsViewModel> Get()
        {
            ResultViewModel<ConditionsAndTermsViewModel> resultViewModel = new ResultViewModel<ConditionsAndTermsViewModel>();
                resultViewModel.Data = _ConditionsAndTermsService.Get(Language);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;

            }

    }

       
}
