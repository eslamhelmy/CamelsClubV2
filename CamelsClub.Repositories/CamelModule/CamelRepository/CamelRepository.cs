using CamelsClub.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class CamelRepository : GenericRepository<Models.Camel> , ICamelRepository
    {
        public CamelRepository(CamelsClubContext context): base(context)
        {

        }
    }
}
