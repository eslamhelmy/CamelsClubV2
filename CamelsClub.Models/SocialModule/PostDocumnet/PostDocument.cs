using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class PostDocument : BaseModel
    {
        public string FileName { get; set; }
       // public string Path { get; set; }
        public string Type { get; set; }
        public int PostID { get; set; }
        public virtual Post Post { get; set; }
    }
}
