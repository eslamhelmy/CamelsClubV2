using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class ApprovedGroup : BaseModel
    {
        public int GroupID { get; set; }
        public virtual Group Group { get; set; }
        public int CompetitionID { get; set; }
        public virtual Competition Competition { get; set; }
        public int Status { get; set; }

    }
}
