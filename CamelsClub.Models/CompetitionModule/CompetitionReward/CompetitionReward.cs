using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CompetitionReward : BaseModel
    {
        public string NameArabic { get; set; }
        public string NameEnglish { get; set; }
        public string Notes { get; set; }
        public int CompetitionID { get; set; }
        public virtual Competition Competition { get; set; }
        //in case of adding kind of company or somebody does not exist on system
        public string SponsorText { get; set; }
        public int? SponsorID { get; set; }
        public virtual User Sponsor { get; set; }
        public string Logo { get; set; }
        public int Rank { get; set; }
    }
}
