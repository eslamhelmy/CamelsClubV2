using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface IRefereeCamelReviewService
    {
        PagingViewModel<RefereeCompetitionInviteViewModel> GetUserListForReferee(int CompetitionID, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        PagingViewModel<RefereeCompetitionInviteViewModel> GetUserListForBossReferee(int CompetitionID, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        void RefereeCamelReviewBossSubmit(List<CamelReviewEditViewModel> viewModels);
        RefereeInvitedUserCamelsViewModel GetInvitedUserApprovedCamelsForBoss(int CompetitionID, int InviteID);
        RefereeInvitedUserCamelsViewModel GetInvitedUserApprovedCamels(int CompetitionID, int InviteID );
        List<CamelReviewEditViewModel> GetApprovedCamelDetails(int CamelCompetitionID, Languages language = Languages.Arabic);
        int GetRefereeIDForThisCompetition(int userID, int camelCompetitionID);
        IEnumerable<ListViewModel> GetCamelSpecifications(Languages language = Languages.Arabic);
        bool IsExist(int camelCompetitionID);
        void SubmitRefreeCamelReview(RefreeCamelReviewCreateViewModel viewmodel);
        CamelsApprovmentStatisticViewModel GetCamelsApprovmentStatistics(int CompetitionID, int InviteID);
        bool CheckCamelSpecification(List<CamelSpecificationViewModel> CamelsSpecificationValues);
        bool NotifyOwner(int competitionID);
        bool CheckCamelReviewdBefore(int CamelCompetitionID);
        bool AllInvitersAreReviewed(int competitionID);
        List<ReviewedCompetitorViewModel> GetAllReviewedCompetitors(int competitionID);
        List<ReviewedCamelViewModel> GetAllReviewedCamels(int competitionID, int competitorID);

    }

}
