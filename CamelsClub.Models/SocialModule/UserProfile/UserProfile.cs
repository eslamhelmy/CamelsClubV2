using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class UserProfile : BaseModel
    {
        public string MainImage { get; set; }
        public string CoverImage { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? GenderID { get; set; }
        public string Address { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ProfileImage> ProfileImages { get; set; }
        public virtual ICollection<ProfileVideo> ProfileVideos { get; set; }
    }
}
