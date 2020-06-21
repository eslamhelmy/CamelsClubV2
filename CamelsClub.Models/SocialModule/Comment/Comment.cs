using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class Comment : BaseModel
    {
        public string Text { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public int PostID { get; set; }
        public virtual Post Post { get; set; }
        public int? ParentCommentID { get; set; }
        public virtual Comment ParentComment { get; set; }

        public virtual ICollection<CommentDocument> CommentDocuments { get; set; }
        public virtual ICollection<CommentUserAction> CommentUserActions { get; set; }
        public virtual ICollection<Comment> ChildComments { get; set; }
        
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
