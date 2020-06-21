using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class LoginCreateViewModel
    {
        public string Phone { get; set; }
    }

    public class AdminLoginCreateViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class TokenResponseViewModel
    {
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string Token { get; set; }
    }

}
