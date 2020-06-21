using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class BlockedFriend : BaseModel
    {
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public int BlockedFriendID { get; set; }
        public virtual User BlockFriend { get; set; }

    }
}
