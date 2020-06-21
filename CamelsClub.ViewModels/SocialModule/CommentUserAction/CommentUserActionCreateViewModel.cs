using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CommentUserActionCreateViewModel
    {
        public int ID { get; set; }
        [IgnoreDataMember]
        public int UserID { get; set; }
        public int CommentID { get; set; }
        public int ActionID { get; set; }
        //in case of like and dislike
        public bool IsActive { get; set; } = true;

    }

    public static partial class CommentUserActionExtensions
    {
        public static CommentUserAction ToModel(this CommentUserActionCreateViewModel viewModel)
        {
            return new CommentUserAction
            {
                ID = viewModel.ID,
                ActionID = viewModel.ActionID,
                CommentID=viewModel.CommentID,
                UserID = viewModel.UserID,
                IsActive = viewModel.IsActive
            };
        }
    }
}
