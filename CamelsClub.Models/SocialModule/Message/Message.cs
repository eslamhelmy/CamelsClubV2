using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class Message : BaseModel
    {
        public string Text { get; set; }
        public int FromUserID { get; set; }
        public virtual User FromUser { get; set; }
        public int ToUserID { get; set; }
        public virtual User ToUser { get; set; }
        public bool IsSeen { get; set; }
    }
}
