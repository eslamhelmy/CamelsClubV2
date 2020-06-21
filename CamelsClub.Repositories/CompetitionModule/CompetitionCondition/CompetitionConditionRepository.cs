using CamelsClub.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class CompetitionConditionRepository : GenericRepository<Models.CompetitionCondition> , ICompetitionConditionRepository
    {
        public CompetitionConditionRepository(CamelsClubContext context): base(context)
        {

        }


        
    }
}
