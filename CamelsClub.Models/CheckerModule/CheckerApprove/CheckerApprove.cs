using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CheckerApprove : BaseModel
    {
    
        public int CompetitionCheckerID { get; set; }
        public virtual CompetitionChecker CompetitionChecker { get; set; }

        public int CamelCompetitionID { get; set; }
        public virtual CamelCompetition CamelCompetition { get; set; }
        public int Status { get; set; }
        public string Notes { get; set; }
        public ICollection<ReviewApprove> Reviews { get; set; }
    }
}
