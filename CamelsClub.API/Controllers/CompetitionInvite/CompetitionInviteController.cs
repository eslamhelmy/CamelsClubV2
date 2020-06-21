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
    public class CompetitionInviteController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly ICompetitionInviteService _CompetitionInviteService;

        public CompetitionInviteController(IUnitOfWork unit, ICompetitionInviteService CompetitionInviteService)
        {
            _unit = unit;
            _CompetitionInviteService = CompetitionInviteService;

        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/CompetitionInvite/GetList")]
        public ResultViewModel<PagingViewModel<CompetitionInviteViewModel>> GetList(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<CompetitionInviteViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<CompetitionInviteViewModel>>();
            var userID = int.Parse(UserID);
            resultViewModel.Data = _CompetitionInviteService.Search(userID, orderBy, isAscending, pageIndex, pageSize, Language);
            resultViewModel.Success = true;
            resultViewModel.Message = Resource.DataLoaded;
            return resultViewModel;
        }


        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/CompetitionInvite/Add")]
        [ValidateViewModel]
        public ResultViewModel<bool> Add(CompetitionInviteCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();

            _CompetitionInviteService.Add(viewModel);

            _unit.Save();
            resultViewModel.Success = true;
            resultViewModel.Data = true;
            resultViewModel.Message = Resource.AddedSuccessfully;


            return resultViewModel;
        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/CompetitionInvite/GetByID")]
        public ResultViewModel<CompetitionInviteViewModel> GetByID(int Id)
        {
            ResultViewModel<CompetitionInviteViewModel> resultViewModel = new ResultViewModel<CompetitionInviteViewModel>();
                if (_CompetitionInviteService.IsExists(Id))
                {
                    var res = _CompetitionInviteService.GetByID(Id);
                    resultViewModel.Success = true;
                    resultViewModel.Data = res;
                    resultViewModel.Message = Resource.DataLoaded;
                }
                else
                {
                    resultViewModel.Success = false;
                    resultViewModel.Message = Resource.NotFound;
                }


                return resultViewModel;
        }


        [HttpPut]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/CompetitionInvite/Edit")]
        [ValidateViewModel]
        public ResultViewModel<bool> Edit(CompetitionInviteEditViewModel viewModel)
        {
            try
            {
                _CompetitionInviteService.Edit(viewModel);
                _unit.Save();
                return new ResultViewModel<bool>(true, Resource.UpdatedSuccessfully, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>( ex.Message);
            }
        }

        [HttpDelete]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/CompetitionInvite/Delete")]
        public ResultViewModel<bool> Delete(int id)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
                if (_CompetitionInviteService.IsExists(id))
                {
                    _CompetitionInviteService.Delete(id);
                    _unit.Save();
                    resultViewModel.Success = true;
                    resultViewModel.Message = Resource.DataDeleted;
                }
                else
                {
                    resultViewModel.Success = false;
                    resultViewModel.Message = Resource.NotFound;
                }

                return resultViewModel;

           }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/CompetitionInvite/RejectCompetition")]
        public ResultViewModel<bool> RejectCompetition(int competitionID)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
                var userID = int.Parse(UserID);
                bool res = _CompetitionInviteService.RejectCompetition(competitionID, userID);
                resultViewModel.Success = true;
                resultViewModel.Data = res;
                resultViewModel.Message = Resource.DataLoaded;

                return resultViewModel;
        }
        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Competitor/Join")]
        public ResultViewModel<bool> Join(CheckerJoinCompetitionCreateViewModel viewModel)
        {
            viewModel.UserID = int.Parse(UserID);
            return new ResultViewModel<bool>(_CompetitionInviteService.JoinCompetition(viewModel), Resource.UpdatedSuccessfully, true);
        }

    }
}
