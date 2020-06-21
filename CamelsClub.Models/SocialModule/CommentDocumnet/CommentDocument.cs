using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CommentDocument : BaseModel
    {
        public string FileName { get; set; }
        //public string Path { get; set; }
        public string Type { get; set; }
        public int CommentID { get; set; }
        public virtual Comment Comment { get; set; }

    }
}
