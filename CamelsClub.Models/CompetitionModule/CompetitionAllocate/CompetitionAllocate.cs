using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CompetitionAllocate : BaseModel
    {
        public int GroupID { get; set; }
        public virtual Group Group { get; set; }
        public int CompetitionCheckerID { get; set; }
        public virtual CompetitionChecker CompetitionChecker { get; set; }
        public bool IsReplaced { get; set; }
        //public string GroupNameArabic { get; set; }
        //public string GroupNameEnglish { get; set; }
    }
}
