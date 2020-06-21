using CamelsClub.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class ApprovedGroupRepository : GenericRepository<Models.ApprovedGroup> , IApprovedGroupRepository
    {
        public ApprovedGroupRepository(CamelsClubContext context): base(context)
        {

        }


        
    }
}
