using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class UserConfirmationMessage : BaseModel
    {
        public string Code { get; set; }
        public bool IsUsed { get; set; }
       // public string UserAgent { get; set; }
       // public string IP { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
    }
}
