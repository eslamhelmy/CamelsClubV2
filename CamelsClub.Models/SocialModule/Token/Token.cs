using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class Token : BaseModel
    {
        public string TokenGUID { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public DateTime ExpireDate { get; set; }
        public string IP { get; set; }
        public string UserAgent { get; set; }
        public string DeviceID { get; set; }
        public bool Active { get; set; }
        public DateTime? LoggedOutDate { get; set; }
        public virtual ICollection<TokenLog> TokenLogs { get; set; }
    }
}
