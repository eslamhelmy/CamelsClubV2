using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public interface IFriendRepository : IGenericRepository<Friend>
    {
        List<int> FriendUsersIDs(int userId);
    }
}
