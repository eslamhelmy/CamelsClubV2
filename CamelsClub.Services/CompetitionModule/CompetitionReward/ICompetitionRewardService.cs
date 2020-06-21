using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface ICompetitionRewardService
    {
        PagingViewModel<CompetitionRewardViewModel> Search(int userID=0,string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        void Add(CompetitionRewardCreateViewModel view);
        void Edit(CompetitionRewardEditViewModel viewModel);
        void Delete(int id);
        CompetitionRewardViewModel GetByID(int id);
        bool IsExists(int id);
        

    }
}
