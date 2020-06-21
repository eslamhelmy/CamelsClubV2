using CamelsClub.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class CompetitionRepository : GenericRepository<Models.Competition> , ICompetitionRepository
    {
        public CompetitionRepository(CamelsClubContext context): base(context)
        {

        }


        
    }
}
