using CamelsClub.Data.Context;
using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class CheckerApproveRepository : GenericRepository<CheckerApprove> , ICheckerApproveRepository
    {
        public CheckerApproveRepository(CamelsClubContext context): base(context)
        {

        }
    }
}
