using CamelsClub.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class CompetitionRewardRepository : GenericRepository<Models.CompetitionReward> , ICompetitionRewardRepository
    {
        public CompetitionRewardRepository(CamelsClubContext context): base(context)
        {

        }


        
    }
}
