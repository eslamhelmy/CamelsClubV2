using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface ICategoryService
    {
        PagingViewModel<CategoryViewModel> Search(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        void Add(CategoryCreateViewModel view);
        void Edit(CategoryCreateViewModel viewModel);
        void Delete(int id);
        CategoryViewModel GetByID(int id);
        bool IsExists(int id);
    }
}
