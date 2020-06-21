using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CamelsClub.Services.Helpers
{
    public interface IApplicationLogService
    {

        bool AddApplicationLog(ApplicationLogCreateViewModel log);
        void NotifyMe(ApplicationLogCreateViewModel log);
    }
}
