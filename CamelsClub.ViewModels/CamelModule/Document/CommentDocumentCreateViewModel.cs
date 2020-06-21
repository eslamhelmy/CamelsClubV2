using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CommentDocumentCreateViewModel
    {
        public int CommentID { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }
}
