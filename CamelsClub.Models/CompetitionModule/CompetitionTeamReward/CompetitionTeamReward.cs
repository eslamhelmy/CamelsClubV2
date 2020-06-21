using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CompetitionTeamReward : BaseModel
    {
        public string TextArabic { get; set; }
        public string TextEnglish { get; set; }
        public int CompetitionID { get; set; }
        public virtual Competition Competition { get; set; }
        public int AssignedTo { get; set; }

    }
}
