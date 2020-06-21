using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class Role : BaseModel
    {
        public string NameArabic { get; set; }
        public string NameEnglish { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }

    }
}
