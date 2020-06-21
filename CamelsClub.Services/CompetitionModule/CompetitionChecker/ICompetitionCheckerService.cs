using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface ICompetitionCheckerService
    {
        PagingViewModel<CompetitionCheckerViewModel> Search(int userID=0,string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        void Add(CompetitionCheckerCreateViewModel view);
        void Edit(CompetitionCheckerCreateViewModel viewModel);
        void Delete(int id);
        CompetitionCheckerViewModel GetByID(int id);
        bool IsExists(int id);
        bool JoinCompetition(CheckerJoinCompetitionCreateViewModel viewModel);
        bool RejectCompetition(CheckerJoinCompetitionCreateViewModel viewModel);
        bool HasJoinedCompetition(CheckerJoinCompetitionCreateViewModel viewModel);
        CheckersReportViewModel GetTeam(CheckerJoinCompetitionCreateViewModel viewModel);
        bool PickupTeam(List<CompetitionCheckerPickupViewModel> viewModels, int loggedUserID);
        bool AutoAllocate(CheckerJoinCompetitionCreateViewModel viewModel);
        bool ManualAllocate(List<ManualAllocateCreateViewModel> viewModels);
        List<GroupViewModel> GetGroups(int competitionID, int loggedUserID);
        List<CamelCompetitionViewModel> GetCamels(int competitionID, int groupID, int loggedUserID);
        bool EvaluateCamel(CheckerApproveCreateViewModel viewModel);
        bool ChangeChecker(ChangeCheckerCreateViewModel viewModel);
        bool ApproveGroup(ApproveGroupCreateViewModel viewModel);
        bool RejectGroup(ApproveGroupCreateViewModel viewModel);
        bool ReviewApprove(ReviewApproveRequestCreateViewModel viewModel, int loggedUserID);
        List<ReviewApproveViewModel> GetReviewRequests(int competitionID, int loggedUserID);
        bool AddApproveReview(ReviewApproveCreateViewModel viewModel, int loggedUserID);
        CheckersReportViewModel GetPickedCheckers(int competitionID, int loggedUserID);
    }
}
