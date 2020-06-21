using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CamelReviewCreateViewModel
    {
        public int UserID { get; set; }  //RefreeUserIDID
        public int CamelCompetitionID { get; set; }
        public int CamelSpecificationID { get; set; }
        public int ActualPercentageValue { get; set; }


    }
}
