using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class Post : BaseModel
    {
        public string Text { get; set; }
        public string Notes { get; set; }
        public int PostType { get; set; }
        public int PostStatus { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<PostUserAction> PostUserActions { get; set; }
        public virtual ICollection<PostDocument> PostDocuments { get; set; }
        public virtual ICollection<IssueReport> IssueReports { get; set; }

    }
}
