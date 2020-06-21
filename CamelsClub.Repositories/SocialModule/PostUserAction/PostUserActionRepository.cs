using CamelsClub.Data.Context;
using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class PostUserActionRepository : GenericRepository<PostUserAction> , IPostUserActionRepository
    {
        public PostUserActionRepository(CamelsClubContext context): base(context)
        {

        }
    }
}
