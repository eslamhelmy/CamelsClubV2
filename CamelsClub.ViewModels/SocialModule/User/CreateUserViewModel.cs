using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CreateUserViewModel
    {
        public int ID { get; set; }

        [RegularExpression(@"^\s*[\w\-\+_]+(\.[\w\-\+_]+)*\@[\w\-\+_]+\.[\w\-\+_]+(\.[\w\-\+_]+)*\s*$", ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "InValidEmail")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public string DisplayName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public string UserName { get; set; }

        [RegularExpression(@"^(05)[0-9]{8}", ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "InValidPhoneNumber")]
        [Required(ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "PhoneRequired")]
        public string MobileNumber { get; set; }

        [RegularExpression(@"^[0-9]{10}", ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "InValidNID")]
        [Required(ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public string NID { get; set; }
    }

    public static class UserExtension
    {
       
        public static User ToUserModel(this CreateUserViewModel viewmodel)
        {
            return new User
            {
                DisplayName = viewmodel.DisplayName,
                UserName = viewmodel.UserName,
                Phone = viewmodel.MobileNumber,
                NID = viewmodel.NID,
                Email=viewmodel.Email,
            };
        }
       
    }
}
