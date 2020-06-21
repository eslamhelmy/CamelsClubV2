using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CamelDocument : BaseModel
    {
        public string FileName { get; set; }
        public string Type { get; set; }
        public int CamelID { get; set; }
        public virtual Camel Camel { get; set; }
    }
}
