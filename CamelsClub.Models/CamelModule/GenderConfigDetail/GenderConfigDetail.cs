using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class GenderConfigDetail : BaseModel
    {
        public string NameArabic { get; set; }
        public string NameEnglish { get; set; }
        public int GenderConfigID { get; set; }
        public GenderConfig GenderConfig { get; set; }
        public virtual ICollection<Camel> Camels { get; set; }
    }
}
