using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CommentUserAction : BaseModel
    {
        public int CommentID { get; set; }
        public virtual Comment Comment { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public int ActionID { get; set; }
        public virtual Action Action { get; set; }
        public bool IsActive { get; set; }
                                           //     public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
