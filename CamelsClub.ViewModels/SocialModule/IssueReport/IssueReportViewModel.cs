using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class IssueReportViewModel
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public string ReportReasonTextArabic { get; set; }
        public string ReportReasonTextEnglish { get; set; }
        public string UserProfileImagePath { get; set; }
        public int PostID { get; set; }

    }
}
