using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class Action : BaseModel
    {
        public string NameArabic { get; set; }
        public string NameEnglish { get; set; }
        public virtual ICollection<PostUserAction> PostUserActions { get; set; }
        public virtual ICollection<CommentUserAction> CommentUserActions { get; set; }

    }
}
