using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class Friend : BaseModel
    {
        public string Notes { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public int FriendUserID { get; set; }
        public virtual User FriendUser { get; set; }
        public bool IsBlocked { get; set; }
     //   public int BlockedBy { get; set; }

    }
}
