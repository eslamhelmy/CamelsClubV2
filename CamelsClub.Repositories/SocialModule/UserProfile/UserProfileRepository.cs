using CamelsClub.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class UserProfileRepository : GenericRepository<Models.UserProfile> , IUserProfileRepository
    {
        public UserProfileRepository(CamelsClubContext context): base(context)
        {

        }
    }
}
