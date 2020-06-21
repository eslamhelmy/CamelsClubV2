using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class Group : BaseModel
    {
        public string NameArabic { get; set; }
        public string NameEnglish { get; set; }
        public string Image { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public bool IsLocked { get; set; }
        public virtual ICollection<CamelCompetition> CamelCompetitions { get; set; }
        public virtual ICollection<CamelGroup> CamelGroups { get; set; }
        public virtual ICollection<CompetitionAllocate> Allocates { get; set; }
        public virtual ICollection<CompetitionRefereeAllocate> RefereeAllocates { get; set; }
        
    }
}
