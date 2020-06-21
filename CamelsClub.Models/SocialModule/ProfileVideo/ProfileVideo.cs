using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class ProfileVideo : BaseModel
    {
        public string FileName { get; set; }
        public int UserProfileID { get; set; }
        public virtual UserProfile UserProfile { get; set; }

    }
}
