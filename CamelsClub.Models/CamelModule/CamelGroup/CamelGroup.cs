using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CamelGroup : BaseModel
    {
        public int CamelID { get; set; }
        public virtual Camel Camel { get; set; }
        public int GroupID { get; set; }
        public virtual Group Group { get; set; }

    }
}
