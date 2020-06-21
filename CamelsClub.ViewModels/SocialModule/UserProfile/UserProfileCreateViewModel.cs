using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class UserProfileCreateViewModel
    {

     
        public int ID { get; set; }
        public string MainImage  { get; set; }
        public string CoverImage  { get; set; }
        public int GenderID { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public List<ProfileImageCreateViewModel> ProfileImages { get; set; }
        public List<ProfileVideoCreateViewModel> ProfileVideos { get; set; }
    }
    public static class UserProfileExtension
    {

        public static UserProfile ToModel(this UserProfileCreateViewModel viewmodel)
        {
            return new UserProfile
            {
                ID = viewmodel.ID,
                CoverImage = viewmodel.CoverImage,
                BirthDate = viewmodel.BirthDate,
                MainImage = viewmodel.MainImage,
                Address=viewmodel.Address,
                GenderID=viewmodel.GenderID
            };
        }

        public static User ToUserModel(this UserProfileCreateViewModel viewmodel)
        {
            return new User
            {
                ID = viewmodel.ID,
                DisplayName=viewmodel.DisplayName,
                Phone=viewmodel.PhoneNumber,
                Email=viewmodel.Email,
                
                
            };
        }
    }
}

