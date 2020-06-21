using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CompetitionSpecification : BaseModel
    {
        public int CamelSpecificationID { get; set; }
        public virtual CamelSpecification CamelSpecification { get; set; }
        public int CompetitionID { get; set; }
        public virtual Competition Competition { get; set; }
        public decimal MaxAllowedValue { get; set; }
    }
}
