using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface IIssueReportService
    {
        PagingViewModel<IssueReportViewModel> Search(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        void Add(IssueReportCreateViewModel view);
        void Edit(IssueReportCreateViewModel viewModel);
        void Delete(int id);
        IssueReportViewModel GetByID(int id);
        bool IsExists(int postID, int userID);
    }
}
