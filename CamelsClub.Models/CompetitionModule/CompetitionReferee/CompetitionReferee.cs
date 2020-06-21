using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CompetitionReferee : BaseModel
    {
        public int CompetitionID { get; set; }
        public virtual Competition Competition { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public bool IsBoss { get; set; }
        public DateTime? JoinDateTime { get; set; }
        public DateTime? RejectDateTime { get; set; }
        public DateTime? PickupDateTime { get; set; }
        

        public virtual ICollection<RefereeCamelSpecificationReview> MyReviews { get; set; } // List of Camels that Reviewed by me as Judger
        public virtual ICollection<CompetitionRefereeAllocate> Allocates { get; set; } // List of Camels that Reviewed by me as Judger
        public DateTime? ChangeByOwnerDateTime { get; set; }
    }
}
