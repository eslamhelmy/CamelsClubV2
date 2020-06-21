using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class PostUserActionViewModel
    {
      
        public int ID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public int ActionID { get; set; }
        public bool IsActive { get; set; }
        public bool Liked { get; set; }

    }
    public  static class PostUserActionExtension
    {

        public static PostUserActionViewModel ToViewModel(this PostUserAction model)
        {
            return new PostUserActionViewModel
            {
                ID = model.ID,
                ActionID = model.ActionID,
                UserID = model.UserID,

            };
        }
    }
}

