using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class RefereeCamelSpecificationReview : BaseModel
    { 
        public int CompetitionRefereeID { get; set; }
        public virtual CompetitionReferee CompetitionReferee { get; set; }  //judgerID
        public int CamelCompetitionID { get; set; }
        public virtual CamelCompetition CamelCompetition { get; set; }
        public int CamelSpecificationID { get; set; }
        public virtual CamelSpecification CamelSpecification { get; set; }
        public double ActualPercentageValue { get; set; }
        public bool Confirmed { get; set; }
    }
}
