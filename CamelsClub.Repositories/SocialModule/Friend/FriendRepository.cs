using CamelsClub.Data.Context;
using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class FriendRepository : GenericRepository<Friend> , IFriendRepository
    {
        public FriendRepository(CamelsClubContext context): base(context)
        {

        }


        public List<int> FriendUsersIDs (int userId)
        {
            return this.GetAll().Where(UF => UF.UserID == userId)
                                .Where(UF => !UF.IsDeleted)
                                .Where(UF => !UF.FriendUser.IsDeleted).Select(UF=>UF.FriendUserID).ToList();
        }
    }
}
