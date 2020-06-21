using CamelsClub.Data.Context;
using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class GenderConfigRepository : GenericRepository<GenderConfig> , IGenderConfigRepository
    {
        public GenderConfigRepository(CamelsClubContext context): base(context)
        {

        }

        public IEnumerable<GenderConfigDetail> GetCamelGender(int age)
        {
            var allowedGenderTypes =
              GetAll().
                  Where(x => x.FromAge <= age && x.ToAge >= age).
                  Where(x=>!x.IsDeleted).
                     SelectMany(x => x.GenderConfigDetails).
                        Where(x=>!x.IsDeleted).
                          ToList();

            return allowedGenderTypes;
        }
    }
}
