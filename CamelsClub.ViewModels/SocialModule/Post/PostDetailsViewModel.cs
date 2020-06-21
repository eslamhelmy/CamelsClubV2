using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class PostDetailsViewModel
    {
        public PostDetailsViewModel()
        {
            Comments = new List<CommentViewModel>();
            Documents= new List<DocumentViewModel>();
        }

        public int ID { get; set; }
        public int UserID { get; set; }

        public string Text { get; set; }
        public string UserName { get; set; }
        public string UserImagePath { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public string Notes { get; set; }
        public int NumberOfLike { get; set; }
        public int NumberOfComments { get; set; }
        public int NumberOfShare { get; set; }
        public PostType PostType { get; set; }
        public bool IsLiked { get; set; }
        public PostStatus PostStatus { get; set; }
        public CommentViewModel LastComment { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
        public IEnumerable<DocumentViewModel> Documents { get; set; }
        public string DisplayName { get; set; }
    }
}
