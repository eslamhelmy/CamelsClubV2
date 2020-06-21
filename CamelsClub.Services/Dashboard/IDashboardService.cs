using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface IDashboardService
    {
        DashboardViewModel GetDashboardData(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20);

    }
}
