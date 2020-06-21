using CamelsClub.Models;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CreatePostViewModel
    {

       public CreatePostViewModel()
        {
          //  Files = new List<PostDocumentCreateViewModel>();
        }
        public int ID { get; set; }
        [IgnoreDataMember]
        public int UserID { get; set; }
        public string Text { get; set; }
        public string Notes { get; set; }
        public PostType PostType { get; set; }
        public PostStatus PostStatus { get; set; }


        public List<PostDocumentCreateViewModel> Files { get; set; }
    }
    public static class PostExtension
    {

        public static Post ToPostModel(this CreatePostViewModel viewmodel)
        {
            return new Post
            {
                Text = viewmodel.Text,
                Notes = viewmodel.Notes,
                UserID = viewmodel.UserID ,
                PostType =(int) viewmodel.PostType,
                PostStatus =(int) viewmodel.PostStatus
     
            };
        }


        public static CreatePostViewModel ToPostViewModel(this Post model)
        {
            return new CreatePostViewModel
            {
                ID=model.ID,
                Text = model.Text,
                Notes = model.Notes,
                UserID = model.UserID,

                

            };
        }

    }

}


