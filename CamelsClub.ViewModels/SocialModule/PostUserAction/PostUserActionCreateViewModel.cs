using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class PostUserActionCreateViewModel
    {
        public int ID { get; set; }
        [IgnoreDataMember]
        public int UserID { get; set; }
        public int PostID { get; set; }
        public int ActionID { get; set; }
        //in case of like and dislike
        public bool IsActive { get; set; } = true;

    }

    public static partial class PostUserActionExtensions
    {
        public static PostUserAction ToModel(this PostUserActionCreateViewModel viewModel)
        {
            return new PostUserAction
            {
                ID = viewModel.ID,
                ActionID = viewModel.ActionID,
                UserID = viewModel.UserID,
                PostID = viewModel.PostID ,
                IsActive = viewModel.IsActive
            };
        }
    }
}
