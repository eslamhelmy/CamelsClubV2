using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class ReportReason : BaseModel
    {
        public string TextArabic { get; set; }
        public string TextEnglish { get; set; }
        public virtual ICollection<IssueReport> IssueReports { get; set; }

    }
}
