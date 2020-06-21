using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class PostUserAction : BaseModel
    {
        public int PostID { get; set; }
        public virtual Post Post { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public int ActionID { get; set; }
        public virtual Action Action { get; set; }
        public bool IsActive { get; set; }
                                           //     public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
