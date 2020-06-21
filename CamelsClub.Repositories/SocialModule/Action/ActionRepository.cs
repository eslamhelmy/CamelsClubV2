using CamelsClub.Data.Context;
using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = CamelsClub.Models.Action;

namespace CamelsClub.Repositories
{
    public class ActionRepository : GenericRepository<Action> , IActionRepository
    {
        public ActionRepository(CamelsClubContext context): base(context)
        {

        }
    }
}
