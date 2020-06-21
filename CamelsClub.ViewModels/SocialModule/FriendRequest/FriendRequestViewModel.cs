using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class FriendRequestViewModel
    {

        public int ID { get; set; }
        public string ToUserName { get; set; }
        public string ToUserMainImagePath { get; set; }
        public int Status { get; set; }
        public string Notes { get; set; }

    }
    public  static class FrienRequestExtension
    {

        public static FriendRequestViewModel ToViewModel(this FriendRequest model)
        {
            return new FriendRequestViewModel
            {
                ID = model.ID,
                ToUserName = model.ToUser?.UserName,
                ToUserMainImagePath = model.ToUser?.UserProfile?.MainImage,
                Notes = model.Notes,
                Status = model.Status
            };
        }
    }
}

