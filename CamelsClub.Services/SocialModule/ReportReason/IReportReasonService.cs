using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface IReportReasonService
    {
        PagingViewModel<ReportReasonViewModel> Search(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        void Add(ReportReasonCreateViewModel view);
        void Edit(ReportReasonCreateViewModel viewModel);
        void Delete(int id);
        ReportReasonViewModel GetByID(int id);
        bool IsExists(int id);
    }
}
