using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CompetitionInvite : BaseModel
    {
        public int CompetitionID { get; set; }
        public virtual Competition Competition { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        //it will checked after submitting all camels of user
        public DateTime? SubmitDateTime { get; set; }
        public DateTime? JoinDateTime { get; set; }
        public DateTime? RejectDateTime { get; set; }
        public int? CheckerID { get; set; }
        public double? FinalScore { get; set; }

        public virtual CompetitionChecker Checker { get; set; }
        public virtual ICollection<CamelCompetition> CamelCompetitions { get; internal set; }

    }
}
