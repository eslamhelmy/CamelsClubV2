using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface ICamelCompetitionService
    {
        PagingViewModel<CamelCompetitionViewModel> Search(int competitionID = 0, int camelID = 0, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        void Add(int userID , List<CamelCompetitionCreateViewModel> CamelCompetitions);
        bool AddGroup(JoinCompetitionCreateViewModel viewModel);

        bool IsGroupExistInCompetition(int competitionID, int groupID);
        //void Edit(CompetitionCreateViewModel viewModel);
        void Delete(int id);
        CamelCompetitionViewModel GetByID(int id, Languages language = Languages.Arabic);
        bool IsExists(int cameID, int competitionID);
    




    }
}
