
using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface ICompetitionRefereeService
    {
        PagingViewModel<CompetitionRefereeViewModel> Search(int userID = 0, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        void Add(CompetitionRefereeCreateViewModel view);
        void Edit(CompetitionRefereeCreateViewModel viewModel);
        void Delete(int id);
        CompetitionRefereeViewModel GetByID(int id);
        bool IsExists(int id);
        bool JoinCompetition(CheckerJoinCompetitionCreateViewModel viewModel);
        bool RejectCompetition(CheckerJoinCompetitionCreateViewModel viewModel);
        bool HasJoinedCompetition(CheckerJoinCompetitionCreateViewModel viewModel);
        List<CompetitionSpecificationViewModel> GetCompetitionSpecifications(int competitionID);
        RefereesReportViewModel GetTeam(CheckerJoinCompetitionCreateViewModel viewModel);
        bool PickupTeam(List<CompetitionCheckerPickupViewModel> viewModels, int loggedUserID);
        bool ManualAllocate(List<RefereeAllocateCreateViewModel> viewModels);
        bool AutoAllocate(CheckerJoinCompetitionCreateViewModel viewModel);
        List<CamelCompetitionRefereeViewModel> GetCamels(int competitionID, int groupID, int loggedUserID);
        List<GroupViewModel> GetGroups(int competitionID, int loggedUserID);
        bool Evaluate(RefreeCamelReviewCreateViewModel viewModel);
        bool ApproveCamel(ApproveCamelCreateViewModel viewModel);
        bool RejectCamel(ApproveCamelCreateViewModel viewModel);
        bool ChangeReferee(ChangeRefereeCreateViewModel viewModel);
        OverallGroupRefereeViewModel GetRefereeCamelsInfo(int competitionID, int groupID, int loggedUserID);
        OverallGroupViewModel GetBossCamelsInfo(int competitionID, int groupID, int loggedUserID);
        List<CamelSpecificationViewModel> GetRefereeCamelSpecifications(int ID, int loggedUserID);
        CamelCompetitionSpecificationBossViewModel GetBossCamelsSpecifications(int ID, int loggedUserID);
        bool ApproveGroup(ApproveGroupCreateViewModel viewModel);
        bool SendCompetitionResult(SendCompetitionResultCreateViewModel viewModel);
        RefereesReportViewModel GetPickedTeam(int competitionID, int loggedUserID);


    }
}
