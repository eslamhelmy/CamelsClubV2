using CamelsClub.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class CamelGroupRepository : GenericRepository<Models.CamelGroup> , ICamelGroupRepository
    {
        public CamelGroupRepository(CamelsClubContext context): base(context)
        {

        }
    }
}
