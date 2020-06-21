using CamelsClub.Data.Context;
using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class ReportReasonRepository : GenericRepository<ReportReason> , IReportReasonRepository
    {
        public ReportReasonRepository(CamelsClubContext context): base(context)
        {

        }
    }
}
