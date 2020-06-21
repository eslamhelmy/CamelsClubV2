using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CamelCompetition : BaseModel
    {
        public int CompetitionInviteID { get; set; }
        public CompetitionInvite CompetitionInvite { get; set; }
        public int CamelID { get; set; }
        public virtual Camel Camel { get; set; }
        public int CompetitionID { get; set; }
        public virtual Competition Competition { get; set; }
        public int GroupID { get; set; }
        public virtual Group Group { get; set; }
        public DateTime? ApprovedByCheckerBossDateTime { get; set; }
        public DateTime? RejectedByCheckerBossDateTime { get; set; }
        public DateTime? ApprovedByRefereeBossDateTime { get; set; }
        public DateTime? RejectedByRefereeBossDateTime { get; set; }
        
        //update whole app
        //public string CamelName { get; set; }
        //public string GroupNameArabic { get; set; }
        //public string GroupNameEnglish { get; set; }
        //public string GroupImage { get; set; }
        //public string CamelImages { get; set; }
        //end
        public virtual ICollection<CheckerApprove> CheckerApprovers { get; internal set; }
        public virtual ICollection<RefereeCamelSpecificationReview> RefereeReviews { get; internal set; }
    }
}
