using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CompetitionRefereeAllocate : BaseModel
    {
        public int GroupID { get; set; }
        public virtual Group Group { get; set; }
        public int CompetitionRefereeID { get; set; }
        public virtual CompetitionReferee CompetitionReferee { get; set; }
        public bool IsReplaced { get; set; }
    }
}
