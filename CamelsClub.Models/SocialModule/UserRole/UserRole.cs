using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class UserRole : BaseModel
    {
        public int RoleID { get; set; }
        public virtual Role Role { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }

    }
}
