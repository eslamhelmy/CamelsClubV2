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
    public class CompetitionRefereeController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly ICompetitionRefereeService _competitionRefereeService;

        public CompetitionRefereeController(IUnitOfWork unit, ICompetitionRefereeService competitionRefereeService)
        {
            _unit = unit;
            _competitionRefereeService = competitionRefereeService;

        }

        
        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Referee/Join")]
        public ResultViewModel<bool> Join(CheckerJoinCompetitionCreateViewModel viewModel)
        {
            try
            {
                if (_competitionRefereeService.HasJoinedCompetition(viewModel))
                {
                    return new ResultViewModel<bool>(true, Resource.UpdatedSuccessfully, true);
                }
                viewModel.UserID = int.Parse(UserID);
                return new ResultViewModel<bool>(_competitionRefereeService.JoinCompetition(viewModel), Resource.UpdatedSuccessfully, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);

            }
        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Referee/Reject")]
        public ResultViewModel<bool> Reject(CheckerJoinCompetitionCreateViewModel viewModel)
        {
            try
            {
                viewModel.UserID = int.Parse(UserID);
                return new ResultViewModel<bool>(_competitionRefereeService.RejectCompetition(viewModel), Resource.UpdatedSuccessfully, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }
        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Referee/ManualAllocate")]
        public ResultViewModel<bool> ManualAllocate(List<RefereeAllocateCreateViewModel> viewModels)
        {
            try
            {
                return new ResultViewModel<bool>(_competitionRefereeService.ManualAllocate(viewModels), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }
        }
        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Referee/AutoAllocate")]
        public ResultViewModel<bool> AutoAllocate(CheckerJoinCompetitionCreateViewModel viewModel)
        {
            try
            {
                viewModel.UserID = int.Parse(UserID);
                return new ResultViewModel<bool>(_competitionRefereeService.AutoAllocate(viewModel), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }
        }


        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Referee/GetTeam")]
        public ResultViewModel<RefereesReportViewModel> GetTeam(CheckerJoinCompetitionCreateViewModel viewModel)
        {
            try
            {
                viewModel.UserID = int.Parse(UserID);
                return new ResultViewModel<RefereesReportViewModel>(_competitionRefereeService.GetTeam(viewModel), Resource.DataLoaded, true);
            }
            catch (Exception ex)
            {
                return new ResultViewModel<RefereesReportViewModel>(ex.Message);
            }
        }


        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Referee/GetCompetitionSpecifications")]
        public ResultViewModel<List<CompetitionSpecificationViewModel>> GetCompetitionSpecifications(int competitionID)
        {
            try
            {
                var userID = int.Parse(UserID);
                return new ResultViewModel<List<CompetitionSpecificationViewModel>>(_competitionRefereeService.GetCompetitionSpecifications(competitionID), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<CompetitionSpecificationViewModel>>(ex.Message);
            }
        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Referee/PickupTeam")]
        public ResultViewModel<bool> PickupTeam(List<CompetitionCheckerPickupViewModel> viewModels)
        {
            try
            {
                var userID = int.Parse(UserID);
                return new ResultViewModel<bool>(_competitionRefereeService.PickupTeam(viewModels, userID), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }
        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Referee/Evaluate")]
        public ResultViewModel<bool> Evaluate(RefreeCamelReviewCreateViewModel viewModel)
        {
            try
            {
                viewModel.UserID = int.Parse(UserID);
                return new ResultViewModel<bool>(_competitionRefereeService.Evaluate(viewModel), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }
        }


        //[HttpPost]
        //[AuthorizeUserFilter(Role = "User")]
        //[Route("api/Referee/ApproveCamel")]
        //public ResultViewModel<bool> ApproveCamel(ApproveCamelCreateViewModel viewModel)
        //{
        //    try
        //    {
        //        viewModel.UserID = int.Parse(UserID);
        //        return new ResultViewModel<bool>(_competitionRefereeService.ApproveCamel(viewModel), Resource.AddedSuccessfully, true);

        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultViewModel<bool>(ex.Message);
        //    }
        //}

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Referee/SendCompetitionResult")]
        public ResultViewModel<bool> SendCompetitionResult(SendCompetitionResultCreateViewModel viewModel)
        {
            try
            {
                viewModel.UserID = int.Parse(UserID);
                return new ResultViewModel<bool>(_competitionRefereeService.SendCompetitionResult(viewModel), Resource.AddedSuccessfully, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }
        }

        //for checker boss to see dashboard for checkers 
        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Referee/GetPickedReferees")]
        public ResultViewModel<RefereesReportViewModel> GetPickedReferees(int competitionID)
        {
            try
            {
                var loggedUserID = int.Parse(UserID);
                return new ResultViewModel<RefereesReportViewModel>(_competitionRefereeService.GetPickedTeam(competitionID, loggedUserID), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<RefereesReportViewModel>(ex.Message);
            }
        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Referee/ApproveGroup")]
        public ResultViewModel<bool> ApproveGroup(ApproveGroupCreateViewModel viewModel)
        {
            try
            {
                viewModel.UserID = int.Parse(UserID);
                return new ResultViewModel<bool>(_competitionRefereeService.ApproveGroup(viewModel), Resource.AddedSuccessfully, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }
        }

        //[HttpPost]
        //[AuthorizeUserFilter(Role = "User")]
        //[Route("api/Referee/RejectCamel")]
        //public ResultViewModel<bool> RejectCamel(ApproveCamelCreateViewModel viewModel)
        //{
        //    try
        //    {
        //        viewModel.UserID = int.Parse(UserID);
        //        return new ResultViewModel<bool>(_competitionRefereeService.RejectCamel(viewModel), Resource.AddedSuccessfully, true);

        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultViewModel<bool>(ex.Message);
        //    }
        //}

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Referee/GetCamels")]
        public ResultViewModel<List<CamelCompetitionRefereeViewModel>> GetCamels(int competitionID, int groupID)
        {
            try
            {
                var loggedUserID = int.Parse(UserID);
                return new ResultViewModel<List<CamelCompetitionRefereeViewModel>>(_competitionRefereeService.GetCamels(competitionID, groupID, loggedUserID), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<CamelCompetitionRefereeViewModel>>(ex.Message);
            }
        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Referee/GetBossCamelsInfo")]
        public ResultViewModel<OverallGroupViewModel> GetBossCamelsInfo(int competitionID, int groupID)
        {
            try
            {
                var loggedUserID = int.Parse(UserID);
                return new ResultViewModel<OverallGroupViewModel>(_competitionRefereeService.GetBossCamelsInfo(competitionID, groupID, loggedUserID), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<OverallGroupViewModel>(ex.Message);
            }
        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Referee/GetRefereeCamelsInfo")]
        public ResultViewModel<OverallGroupRefereeViewModel> GetRefereeCamelsInfo(int competitionID, int groupID)
        {
            try
            {
                var loggedUserID = int.Parse(UserID);
                return new ResultViewModel<OverallGroupRefereeViewModel>(_competitionRefereeService.GetRefereeCamelsInfo(competitionID, groupID, loggedUserID), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<OverallGroupRefereeViewModel>(ex.Message);
            }
        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Referee/GetBossCamelSpecifications")]
        public ResultViewModel<CamelCompetitionSpecificationBossViewModel> GetBossCamelSpecifications(int ID)
        {
            try
            {
                var loggedUserID = int.Parse(UserID);
                return new ResultViewModel<CamelCompetitionSpecificationBossViewModel>(_competitionRefereeService.GetBossCamelsSpecifications(ID, loggedUserID), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<CamelCompetitionSpecificationBossViewModel>(ex.Message);
            }
        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Referee/GetRefereeCamelSpecifications")]
        public ResultViewModel<List<CamelSpecificationViewModel>> GetRefereeCamelSpecifications(int ID)
        {
            try
            {
                var loggedUserID = int.Parse(UserID);
                return new ResultViewModel<List<CamelSpecificationViewModel>>(_competitionRefereeService.GetRefereeCamelSpecifications(ID, loggedUserID), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<CamelSpecificationViewModel>>(ex.Message);
            }
        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Referee/GetGroups")]
        public ResultViewModel<List<GroupViewModel>> GetGroups(int competitionID)
        {
            try
            {
                var loggedUserID = int.Parse(UserID);
                return new ResultViewModel<List<GroupViewModel>>(_competitionRefereeService.GetGroups(competitionID, loggedUserID), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<GroupViewModel>>(ex.Message);
            }
        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Referee/Change")]
        public ResultViewModel<bool> ChangeReferee(ChangeRefereeCreateViewModel viewModel)
        {
            try
            {
                viewModel.UserID = int.Parse(UserID);
                return new ResultViewModel<bool>(_competitionRefereeService.ChangeReferee(viewModel), Resource.AddedSuccessfully, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }
        }


    }
}
