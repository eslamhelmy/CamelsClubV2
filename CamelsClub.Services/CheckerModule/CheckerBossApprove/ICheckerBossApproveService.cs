using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface ICheckerBossApproveService
    {
        PagingViewModel<CheckerApproveViewModel> GetUpToApprovalRequests(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        PagingViewModel<CheckerApproveViewModel> GetReplacedCamelsRequests(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        void EditingRejectedCamel(EditRejectedCamelCreateViewModel viewModel);
        void ApproveCamel(CheckerBossApprovalCreateViewModel viewModel);
        void RejectCamel(CheckerBossApprovalCreateViewModel viewModel);
        void TerminateCamel(CheckerBossApprovalCreateViewModel viewModel);
        List<CheckerApproveViewModel> GetUserRejectedCamels(int userID, Languages language = Languages.Arabic);
        void ReplaceRejectedCamel(ReplaceRejectedCamelCreateViewModel viewModel);

    }
}
