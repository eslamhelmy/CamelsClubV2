using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface ICompetitionService
    {
        PagingViewModel<CompetitionViewModel> Search(int userID=0,string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        PagingViewModel<CompetitionViewModel> GetMyCompetitons(int userId, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
     
        bool Add(CompetitionCreateViewModel view);
        void Edit(CompetitionEditViewModel viewModel);
        void Delete(int id);
        CompetitionViewModel GetByID(int loggedUser,int id);
        CompetitionEditViewModel GetEditableByID(int id);
        bool IsExists(int id);
        bool IsAllowedToEdit(int iD);
        PagingViewModel<CompetitionViewModel> GetCurrentInvolvedCompetitions(int userID, string orderBy, bool isAscending, int pageIndex, int pageSize, Languages language);
        bool PublishCompetition(int userID, int competitionID);
        bool StartCompetition(int userID, int competitionID);
        bool ChangeRefereeBoss(ChangeRefereeBossCreateViewModel viewModel);
        bool ChangeCheckerBoss(ChangeCheckerBossCreateViewModel viewModel);
        int GetSuspendCompetitionsCount(int userID);
        bool InviteCompetitors(int userID, int competitionID);
    }
}
