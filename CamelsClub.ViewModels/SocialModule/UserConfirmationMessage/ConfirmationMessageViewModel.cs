using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class ConfirmationMessageViewModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string ProfileImagePath { get; set; }
        public string Phone { get; set; }
        public string Code { get; set; }
        
    }
}
