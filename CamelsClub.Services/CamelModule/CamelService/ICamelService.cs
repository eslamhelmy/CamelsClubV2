using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface ICamelService
    {
        PagingViewModel<CamelViewModel> Search(int userID=0,string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        void Add(CamelCreateViewModel view);
        void Edit(CamelCreateViewModel viewModel);
        void Delete(int id);
        CamelViewModel GetByID(int id);
        bool IsExists(int id);
        IEnumerable<GenderConfigDetailViewModel> GetAllowdCamelGenderTypes(DateTime birthDate);

    }
}
