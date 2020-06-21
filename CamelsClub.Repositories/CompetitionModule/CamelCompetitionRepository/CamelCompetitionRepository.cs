using CamelsClub.Data.Context;
using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class CamelCompetitionRepository : GenericRepository<Models.CamelCompetition> , ICamelCompetitionRepository
    {
        public CamelCompetitionRepository(CamelsClubContext context): base(context)
        {

        }


        public bool CheckCamelCompetition(CamelCompetitionCreateViewModel obj)
        {
            return GetAll().Where(x => x.CamelID == obj.CamelID && x.CompetitionID == obj.CompetitionID && !x.IsDeleted).Any();
        }
    }
}
