using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CamelSpecification:BaseModel
    {
        public string SpecificationEnglish { get; set; }
        public string SpecificationArabic { get; set; }
        public virtual ICollection<RefereeCamelSpecificationReview> RefereeReviews { get; internal set; }
        public virtual ICollection<CompetitionSpecification> CompetitionSpecifications { get; internal set; }
        
    }
}
