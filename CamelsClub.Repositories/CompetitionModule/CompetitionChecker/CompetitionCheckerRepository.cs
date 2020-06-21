using CamelsClub.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class CompetitionCheckerRepository : GenericRepository<Models.CompetitionChecker> , ICompetitionCheckerRepository
    {
        public CompetitionCheckerRepository(CamelsClubContext context): base(context)
        {

        }


        
    }
}
