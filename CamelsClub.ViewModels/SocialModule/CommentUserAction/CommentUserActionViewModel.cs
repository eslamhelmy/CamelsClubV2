using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CommentUserActionViewModel
    {
      
        public int ID { get; set; }
        public int CommentID { get; set; }
        public int UserID { get; set; }
        public int ActionID { get; set; }
    }
    public  static class CommentUserActionExtension
    {

        public static CommentUserActionViewModel ToViewModel(this CommentUserAction model)
        {
            return new CommentUserActionViewModel
            {
                ID = model.ID,
                ActionID = model.ActionID,
                UserID = model.UserID,

            };
        }
    }
}

