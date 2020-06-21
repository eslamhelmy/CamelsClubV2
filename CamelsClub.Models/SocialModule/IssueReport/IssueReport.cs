using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class IssueReport : BaseModel
    {
        public string Text { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }

        public int PostID { get; set; }
        public virtual Post Post { get; set; }
        public int ReportReasonID { get; set; }
        public virtual ReportReason ReportReason { get; set; }


    }
}
