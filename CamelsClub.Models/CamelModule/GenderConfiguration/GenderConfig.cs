using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class GenderConfig : BaseModel
    {
        public int FromAge { get; set; }
        public int ToAge { get; set; }
        public string Notes { get; set; }
        public virtual ICollection<GenderConfigDetail> GenderConfigDetails { get; set; }
    }
}
