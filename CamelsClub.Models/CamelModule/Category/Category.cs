using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class Category : BaseModel
    {
        public string NameArabic { get; set; }
        public string NameEnglish { get; set; }
        public virtual ICollection<Camel> Camels { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Competition> Competitions { get; set; }

    }
}
