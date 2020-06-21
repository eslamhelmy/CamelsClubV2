using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CompetitionChecker : BaseModel
    {
        public int CompetitionID { get; set; }
        public virtual Competition Competition { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public bool IsBoss { get; set; }
        public DateTime? JoinDateTime { get; set; }
        public DateTime? PickupDateTime { get; set; }
        public DateTime? RejectDateTime { get; set; }

        public virtual ICollection<CompetitionInvite> Invites { get; set; }
        public virtual ICollection<CheckerApprove> CamelsIApproved { get; set; }
        public virtual ICollection<CompetitionAllocate> Allocates { get; set; }
        public DateTime? ChangeByOwnerDateTime { get; set; }
    }
}
