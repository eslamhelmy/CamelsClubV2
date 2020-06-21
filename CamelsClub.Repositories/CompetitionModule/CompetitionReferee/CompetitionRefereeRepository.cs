using CamelsClub.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class CompetitionRefereeRepository : GenericRepository<Models.CompetitionReferee> , ICompetitionRefereeRepository
    {
        public CompetitionRefereeRepository(CamelsClubContext context): base(context)
        {

        }


        
    }
}
