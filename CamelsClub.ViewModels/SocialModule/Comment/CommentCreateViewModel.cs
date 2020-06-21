using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CommentCreateViewModel
    {

     
        public int ID { get; set; }
        [Required(ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int PostID { get; set; }
        [IgnoreDataMember]
        public int UserID { get; set; }
        [Required(ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public string Text { get; set; }
        public int? ParentCommentID { get; set; }

        public List<CommentDocumentCreateViewModel> Files { get; set; }
    }
    public static class CommentExtension
    {

        public static Comment ToCommentModel(this CommentCreateViewModel viewmodel)
        {
            return new Comment
            {
                ID = viewmodel.ID,
                Text = viewmodel.Text,
                PostID = viewmodel.PostID,
                UserID = viewmodel.UserID,
                ParentCommentID = viewmodel.ParentCommentID
            };
        }

    }
}

