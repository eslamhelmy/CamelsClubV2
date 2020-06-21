using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class ReviewApprove : BaseModel
    {
    
        public int CheckerID { get; set; }
        public virtual CompetitionChecker CompetitionChecker { get; set; }

        public int CheckerApproveID { get; set; }
        public virtual CheckerApprove CheckerApprove { get; set; }
        public int Status { get; set; }
        public string Notes { get; set; }

    }
}
