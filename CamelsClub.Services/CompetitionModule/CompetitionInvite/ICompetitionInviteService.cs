using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface ICompetitionInviteService
    {
        PagingViewModel<CompetitionInviteViewModel> Search(int userID=0,string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        void Add(CompetitionInviteCreateViewModel view);
        void Edit(CompetitionInviteEditViewModel viewModel);
        void Delete(int id);
        CompetitionInviteViewModel GetByID(int id);
        bool IsExists(int id);
        bool RejectCompetition(int competitionID, int userID);
        bool JoinCompetition(CheckerJoinCompetitionCreateViewModel viewModel);

    }
}
