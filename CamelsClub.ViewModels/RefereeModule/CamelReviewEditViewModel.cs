using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CamelReviewEditViewModel
    {
        public int ID { get; set; }
        public int CompetitionRefereeID { get; set; }
        public int CamelCompetitionID { get; set; }
        public int CamelSpecificationID { get; set; }
        public double ActualPercentageValue { get; set; }
        
    }
}
