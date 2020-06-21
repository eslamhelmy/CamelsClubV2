using CamelsClub.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class CompetitionAllocateRepository : GenericRepository<Models.CompetitionAllocate> , ICompetitionAllocateRepository
    {
        public CompetitionAllocateRepository(CamelsClubContext context): base(context)
        {

        }


        
    }
}
