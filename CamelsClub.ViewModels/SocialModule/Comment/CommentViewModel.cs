using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CommentViewModel
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public string UserProfileImagePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool HasReplies { get; set; }
        public int RepliesCount { get; set; }
        public int NumberOfLikes { get; set; }
        
        public IEnumerable<DocumentViewModel> Documents { get; set; }
        public IEnumerable<CommentViewModel> Replies { get; set; }
    }
}
