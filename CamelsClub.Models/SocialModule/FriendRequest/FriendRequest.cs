using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class FriendRequest : BaseModel
    {
        public string Notes { get; set; }
        public int Status { get; set; }
        public int FromUserID { get; set; }
        public virtual User FromUser { get; set; }
        public int ToUserID { get; set; }
        public virtual User ToUser { get; set; }

    }
}
