using CamelsClub.API.Filters;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Services;
using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace CamelsClub.API.Controllers
{
    public class CompetitionCheckerController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly ICompetitionCheckerService _CompetitionCheckerService;

        public CompetitionCheckerController(IUnitOfWork unit, ICompetitionCheckerService CompetitionCheckerService)
        {
            _unit = unit;
            _CompetitionCheckerService = CompetitionCheckerService;

        }

        
        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Checker/Join")]
        public ResultViewModel<bool> Join(CheckerJoinCompetitionCreateViewModel viewModel)
        {
            try
            {
                if (_CompetitionCheckerService.HasJoinedCompetition(viewModel))
                {
                    return new ResultViewModel<bool>(true, Resource.UpdatedSuccessfully, true);
                }
                viewModel.UserID = int.Parse(UserID);
                return new ResultViewModel<bool>(_CompetitionCheckerService.JoinCompetition(viewModel), Resource.UpdatedSuccessfully, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }
        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Checker/Reject")]
        public ResultViewModel<bool> Reject(CheckerJoinCompetitionCreateViewModel viewModel)
        {
            try
            {
                viewModel.UserID = int.Parse(UserID);
                return new ResultViewModel<bool>(_CompetitionCheckerService.RejectCompetition(viewModel), Resource.UpdatedSuccessfully, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }
        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Checker/AddReview")]
        public ResultViewModel<bool> AddReview(ReviewApproveCreateViewModel viewModel)
        {
            try
            {
                var userID = int.Parse(UserID);
                return new ResultViewModel<bool>(_CompetitionCheckerService.AddApproveReview(viewModel, userID), Resource.UpdatedSuccessfully, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }
        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Checker/GetTeam")]
        public ResultViewModel<CheckersReportViewModel> GetTeam(CheckerJoinCompetitionCreateViewModel viewModel)
        {
            try
            {
                viewModel.UserID = int.Parse(UserID);
                return new ResultViewModel<CheckersReportViewModel>(_CompetitionCheckerService.GetTeam(viewModel), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<CheckersReportViewModel>(ex.Message);
            }
        }

        //for checker boss to see dashboard for checkers 
        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Checker/GetPickedCheckers")]
        public ResultViewModel<CheckersReportViewModel> GetPickedCheckers(int competitionID)
        {
            try
            {
                var loggedUserID = int.Parse(UserID);
                return new ResultViewModel<CheckersReportViewModel>(_CompetitionCheckerService.GetPickedCheckers(competitionID, loggedUserID), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<CheckersReportViewModel>(ex.Message);
            }
        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Checker/GetGroups")]
        public ResultViewModel<List<GroupViewModel>> GetGroups(int competitionID)
        {
            try
            {
                var loggedUserID = int.Parse(UserID);
                return new ResultViewModel<List<GroupViewModel>>(_CompetitionCheckerService.GetGroups(competitionID, loggedUserID), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<GroupViewModel>>(ex.Message);
            }
        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Checker/GetReviewRequests")]
        public ResultViewModel<List<ReviewApproveViewModel>> GetReviewRequests(int competitionID)
        {
            try
            {
                var loggedUserID = int.Parse(UserID);
                return new ResultViewModel<List<ReviewApproveViewModel>>(_CompetitionCheckerService.GetReviewRequests(competitionID, loggedUserID), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<ReviewApproveViewModel>>(ex.Message);
            }
        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Checker/GetCamels")]
        public ResultViewModel<List<CamelCompetitionViewModel>> GetCamels(int competitionID, int groupID)
        {
            try
            {
                var loggedUserID = int.Parse(UserID);
                return new ResultViewModel<List<CamelCompetitionViewModel>>(_CompetitionCheckerService.GetCamels(competitionID, groupID, loggedUserID), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<CamelCompetitionViewModel>>(ex.Message);

            }
        }


        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Checker/EvaluateCamel")]
        public ResultViewModel<bool> EvaluateCamel(CheckerApproveCreateViewModel viewModel)
        {
            try
            {
                viewModel.UserID = int.Parse(UserID);
                return new ResultViewModel<bool>(_CompetitionCheckerService.EvaluateCamel(viewModel), Resource.AddedSuccessfully, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);

            }
        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Checker/ReviewApprove")]
        public ResultViewModel<bool> ReviewApprove(ReviewApproveRequestCreateViewModel viewModel)
        {
            try
            {
                var loggedUserID = int.Parse(UserID);
                return new ResultViewModel<bool>(_CompetitionCheckerService.ReviewApprove(viewModel,loggedUserID), Resource.AddedSuccessfully, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);

            }
        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Checker/ApproveGroup")]
        public ResultViewModel<bool> ApproveGroup(ApproveGroupCreateViewModel viewModel)
        {
            try
            {
                viewModel.UserID = int.Parse(UserID);
                return new ResultViewModel<bool>(_CompetitionCheckerService.ApproveGroup(viewModel), Resource.AddedSuccessfully, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);

            }
        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Checker/RejectGroup")]
        public ResultViewModel<bool> RejectGroup(ApproveGroupCreateViewModel viewModel)
        {
            try
            {
                viewModel.UserID = int.Parse(UserID);
                return new ResultViewModel<bool>(_CompetitionCheckerService.RejectGroup(viewModel), Resource.AddedSuccessfully, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);

            }
        }


        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Checker/Change")]
        public ResultViewModel<bool> ChangeChecker(ChangeCheckerCreateViewModel viewModel)
        {
            try
            {
                viewModel.UserID = int.Parse(UserID);
                return new ResultViewModel<bool>(_CompetitionCheckerService.ChangeChecker(viewModel), Resource.AddedSuccessfully, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);

            }
        }


        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Checker/AutoAllocate")]
        public ResultViewModel<bool> AutoAllocate(CheckerJoinCompetitionCreateViewModel viewModel)
        {
            try
            {
                viewModel.UserID = int.Parse(UserID);
                return new ResultViewModel<bool>(_CompetitionCheckerService.AutoAllocate(viewModel), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);

            }
        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Checker/ManualAllocate")]
        public ResultViewModel<bool> ManualAllocate(List<ManualAllocateCreateViewModel> viewModels)
        {
            try
            {
                return new ResultViewModel<bool>(_CompetitionCheckerService.ManualAllocate(viewModels), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }
        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Checker/PickupTeam")]
        public ResultViewModel<bool> PickupTeam(List<CompetitionCheckerPickupViewModel> viewModels)
        {
            try
            {
                var loggedUserID = int.Parse(UserID);
                return new ResultViewModel<bool>(_CompetitionCheckerService.PickupTeam(viewModels, loggedUserID), Resource.DataLoaded, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }
        }

    }
}
