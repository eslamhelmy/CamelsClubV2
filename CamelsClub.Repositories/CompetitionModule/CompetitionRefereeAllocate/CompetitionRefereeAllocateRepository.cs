using CamelsClub.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class CompetitionRefereeAllocateRepository : GenericRepository<Models.CompetitionRefereeAllocate> , ICompetitionRefereeAllocateRepository
    {
        public CompetitionRefereeAllocateRepository(CamelsClubContext context): base(context)
        {

        }


        
    }
}
