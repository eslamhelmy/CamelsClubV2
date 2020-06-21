using CamelsClub.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class CompetitionInviteRepository : GenericRepository<Models.CompetitionInvite> , ICompetitionInviteRepository
    {
        public CompetitionInviteRepository(CamelsClubContext context): base(context)
        {

        }


        
    }
}
