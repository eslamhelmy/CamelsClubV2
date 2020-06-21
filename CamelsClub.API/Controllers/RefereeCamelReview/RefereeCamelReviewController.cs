using CamelsClub.API.Filters;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Services;
using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CamelsClub.API.Controllers
{
    public class RefereeCamelReviewController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly IRefereeCamelReviewService _refereeCamelReviewService;

        public RefereeCamelReviewController(IUnitOfWork unit, IRefereeCamelReviewService refereeCamelReviewService)
        {
            _unit = unit;
            _refereeCamelReviewService = refereeCamelReviewService;

        }
       
        //[HttpGet]
        //[AuthorizeUserFilter(Role = "User")]
        //[Route("api/Referee/NotifyOwner")]
        //public ResultViewModel<bool> NotifyOwner(int competitionID)
        //{
        //    var userID = int.Parse(UserID);
        //    ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
        //    resultViewModel.Data = _refereeCamelReviewService.NotifyOwner(competitionID);
        //    //resultViewModel.Success = true;
        //    resultViewModel.Message = Resource.CompetitionIsFinished;
        //    return resultViewModel;
        //}
      
        //// get all competitors that have passed by the checker boss
        //[HttpGet]
        //[AuthorizeUserFilter(Role = "User")]
        //[Route("api/Referee/GetCompetitorsList")]
        //public ResultViewModel<PagingViewModel<RefereeCompetitionInviteViewModel>> GetInvitedUserList(int CompetitionID,string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        //{
        //    var userID = int.Parse(UserID);
        //    ResultViewModel<PagingViewModel<RefereeCompetitionInviteViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<RefereeCompetitionInviteViewModel>>();
        //    resultViewModel.Data = _refereeCamelReviewService.GetUserListForReferee(CompetitionID, orderBy, isAscending, pageIndex, pageSize, Language);
        //    resultViewModel.Success = true;
        //    resultViewModel.Message = Resource.DataLoaded;
        //    return resultViewModel;
        //}

        // get all competitors for boss to review them after they reviewed by other referees
        //[HttpGet]
        //[AuthorizeUserFilter(Role = "User")]
        //[Route("api/Referee/Boss/GetCompetitorsList")]
        //public ResultViewModel<PagingViewModel<RefereeCompetitionInviteViewModel>> GetUserListForBossReferee(int CompetitionID, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        //{
        //    var userID = int.Parse(UserID);
        //    ResultViewModel<PagingViewModel<RefereeCompetitionInviteViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<RefereeCompetitionInviteViewModel>>();
        //    resultViewModel.Data = _refereeCamelReviewService.GetUserListForBossReferee(CompetitionID, orderBy, isAscending, pageIndex, pageSize, Language);
        //    resultViewModel.Success = true;
        //    resultViewModel.Message = Resource.DataLoaded;
        //    return resultViewModel;
        //}
        
        ////competitor not submitted yet so the boss wants his camels to submit
        //[HttpGet]
        //[AuthorizeUserFilter(Role = "User")]
        //[Route("api/Referee/Boss/GetCompetitorCamels")]
        //public ResultViewModel<RefereeInvitedUserCamelsViewModel> GetInvitedUserApprovedCamelsForBoss(int CompetitionID, int InvitedUserID)
        //{
        //    ResultViewModel<RefereeInvitedUserCamelsViewModel> resultViewModel = new ResultViewModel<RefereeInvitedUserCamelsViewModel>();
        //    //  var camelsStatistics = _refereeCamelReviewService.GetCamelsApprovmentStatistics(CompetitionID, InvitedUserID);
        //    resultViewModel.Data = _refereeCamelReviewService.GetInvitedUserApprovedCamelsForBoss(CompetitionID, InvitedUserID);
        //    //   resultViewModel.Data = new { Camels = approvedcamellist,NumberReviewedCamels = camelsStatistics.NumberofReviewedCamels,NumberofWaitingCamels= camelsStatistics.NumberofWatingCamels };

        //    resultViewModel.Success = true;
        //    resultViewModel.Message = Resource.DataLoaded;
        //    return resultViewModel;
        //}

        //// get competitor camels after passing the checking module[step 2 in UI] 
        //[HttpGet]
        //[AuthorizeUserFilter(Role = "User")]
        //[Route("api/Referee/GetCompetitorCamels")]
        //public ResultViewModel<RefereeInvitedUserCamelsViewModel> GetApprovedCamelsList(int CompetitionID,int InvitedUserID)
        //{
        //    ResultViewModel<RefereeInvitedUserCamelsViewModel> resultViewModel = new ResultViewModel<RefereeInvitedUserCamelsViewModel>();
        //  //  var camelsStatistics = _refereeCamelReviewService.GetCamelsApprovmentStatistics(CompetitionID, InvitedUserID);
        //    resultViewModel.Data = _refereeCamelReviewService.GetInvitedUserApprovedCamels(CompetitionID, InvitedUserID);
        //    //   resultViewModel.Data = new { Camels = approvedcamellist,NumberReviewedCamels = camelsStatistics.NumberofReviewedCamels,NumberofWaitingCamels= camelsStatistics.NumberofWatingCamels };

        //    resultViewModel.Success = true;
        //    resultViewModel.Message = Resource.DataLoaded;
        //    return resultViewModel;
        //}

        ////get reviewed camel details with all specification
        //[HttpGet]
        //[AuthorizeUserFilter(Role = "User")]
        //[Route("api/Referee/Boss/GetReviewedCamel")]
        //public ResultViewModel<List<CamelReviewEditViewModel>> GetReviewedCamel(int camelID) //note it is camel competition id 
        //{
        //    ResultViewModel<List<CamelReviewEditViewModel>> resultViewModel = new ResultViewModel<List<CamelReviewEditViewModel>>();
        //   // if (_refereeCamelReviewService.IsExist(camelID))
        //   // {
        //        resultViewModel.Data = _refereeCamelReviewService.GetApprovedCamelDetails(camelID, Language);
        //        resultViewModel.Success = true;
        //        resultViewModel.Message = Resource.DataLoaded;
        //    //}

        //    //else
        //    //{
        //    //    resultViewModel.Success = false;
        //    //    resultViewModel.Message = Resource.NotFound;
        //    //}
        //    return resultViewModel;
        //}

        
        ////get all camels reviewed for specific competitor
        //[HttpGet]
        //[AuthorizeUserFilter(Role = "User")]
        //[Route("api/Referee/Boss/GetAllReviewedCamels")]
        //public ResultViewModel<List<ReviewedCamelViewModel>> GetAllReviewedCamels(int competitionID, int competitorID)
        //{
        //    ResultViewModel<List<ReviewedCamelViewModel>> resultViewModel = new ResultViewModel<List<ReviewedCamelViewModel>>();

        //    resultViewModel.Data = _refereeCamelReviewService.GetAllReviewedCamels(competitionID,competitorID);
        //    resultViewModel.Success = true;
        //    resultViewModel.Message = Resource.DataLoaded;

        //    return resultViewModel;
        //}

        //// get all competitors that have been reviewed by boss
        //[HttpGet]
        //[AuthorizeUserFilter(Role = "User")]
        //[Route("api/Referee/Boss/GetReviewedCompetitors")]
        //public ResultViewModel<List<ReviewedCompetitorViewModel>> GetReviewedCompetitors(int competitionID)
        //{
        //    ResultViewModel<List<ReviewedCompetitorViewModel>> resultViewModel = new ResultViewModel<List<ReviewedCompetitorViewModel>>();
        //    if (_refereeCamelReviewService.AllInvitersAreReviewed(competitionID))
        //    {
        //        resultViewModel.Data = _refereeCamelReviewService.GetAllReviewedCompetitors(competitionID);
        //        resultViewModel.Success = true;
        //        resultViewModel.Message = Resource.DataLoaded;

        //    }
        //    else
        //    {
        //        resultViewModel.Success = false;
        //        resultViewModel.Message = Resource.DataLoaded;

        //    }

        //    return resultViewModel;
        //}

        //get all specifications 
        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Camel/GetSpecifications")]
        public ResultViewModel<IEnumerable<ListViewModel>> GetSpecifications()
        {
            ResultViewModel<IEnumerable<ListViewModel>> resultViewModel = new ResultViewModel<IEnumerable<ListViewModel>>();

            resultViewModel.Data = _refereeCamelReviewService.GetCamelSpecifications(Language);
            resultViewModel.Success = true;
            resultViewModel.Message = Resource.DataLoaded;

            return resultViewModel;
        }

        //confirm competitor review by boss
        //[HttpPost]
        //[AuthorizeUserFilter(Role = "User")]
        //[Route("api/Referee/Boss/ConfirmReview")]
        //public ResultViewModel<bool> RefereeCamelReviewBossSubmit(List<CamelReviewEditViewModel> viewModels)
        //{


        //    ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
              
        //        if (viewModels.Count == 0)
        //        {
        //            resultViewModel.Success = false;
        //            resultViewModel.Message = Resource.CamelsSpecificationRequired;
        //            return resultViewModel;
        //        }
                  
        //        if (!_refereeCamelReviewService.CheckCamelSpecification(viewModels.Select(x=>new CamelSpecificationViewModel{
        //            CamelSpecificationID = x.CamelSpecificationID,
        //            SpecificationValue = x.ActualPercentageValue
        //        }).ToList()))
        //        {
        //            resultViewModel.Success = false;
        //            resultViewModel.Message = Resource.camelspecificationcount;
        //            return resultViewModel;
        //        }
        //        if (viewModels.Sum(spec => spec.ActualPercentageValue) > 100)
        //        {
        //            resultViewModel.Success = false;
        //            resultViewModel.Message = Resource.CamelTotalsSpecifcationsValue;
        //            return resultViewModel;
        //        }

        //        _refereeCamelReviewService.RefereeCamelReviewBossSubmit(viewModels);
        //        resultViewModel.Success = true;
        //        resultViewModel.Data = true;
        //        resultViewModel.Message = Resource.reviewsubmitsucessfully;

        //        return resultViewModel;

        //}
        //add competitor review by referee
        //[HttpPost]
        //[AuthorizeUserFilter(Role = "User")]
        //[Route("api/Referee/AddReview")]
        //public ResultViewModel<bool> RefereeCamelReviewSubmit(RefreeCamelReviewCreateViewModel viewModel)
        //{

        //    ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
        //        viewModel.UserID = int.Parse(UserID);

        //        if (_refereeCamelReviewService.GetRefereeIDForThisCompetition(viewModel.UserID, viewModel.CamelCompetitionID) == 0)
        //        {
        //            resultViewModel.Success = false;
        //            resultViewModel.Message = Resource.NotRefree;
        //            return resultViewModel;
        //        }

        //        if (viewModel.CamelsSpecificationValues.Count == 0)
        //        {
        //            resultViewModel.Success = false;
        //            resultViewModel.Message = Resource.CamelsSpecificationRequired;
        //            return resultViewModel;
        //        }
        //        if (viewModel.CamelsSpecificationValues.Count > 0)
        //        {
        //            if (!_refereeCamelReviewService.CheckCamelSpecification(viewModel.CamelsSpecificationValues))
        //            {
        //                resultViewModel.Success = false;
        //                resultViewModel.Message = Resource.camelspecificationcount;
        //                return resultViewModel;
        //            }
        //            if (viewModel.CamelsSpecificationValues.Sum(spec => spec.SpecificationValue) > 100)
        //            {
        //                resultViewModel.Success = false;
        //                resultViewModel.Message = Resource.CamelTotalsSpecifcationsValue;
        //                return resultViewModel;
        //            }

        //        }

        //        if (_refereeCamelReviewService.CheckCamelReviewdBefore(viewModel.CamelCompetitionID))
        //        {
        //            resultViewModel.Success = false;
        //            resultViewModel.Message = Resource.camelreviewedbefore;
        //            return resultViewModel;
        //        }

        //        _refereeCamelReviewService.SubmitRefreeCamelReview(viewModel);
        //        resultViewModel.Success = true;
        //        resultViewModel.Data = true;
        //        resultViewModel.Message = Resource.reviewsubmitsucessfully;

        //        return resultViewModel;

            
        //}

    }
}
