using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class FriendViewModel
    {
        public int ID { get; set; }
        public string FriendUserName { get; set; }
        public string UserName { get; set; }
        public string FriendMainImagePath { get; set; }
        public string UserMainImagePath { get; set; }
        public int UserID { get; set; }
        public int FriendUserID { get; set; }
        public string DisplayName { get; set; }
    }
    public class ClearedFriendViewModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string MainImagePath { get; set; }
        public int UserID { get; set; }
        public string DisplayName { get; set; }
    }
    public  static class FriendExtension
    {

        public static FriendViewModel ToViewModel(this Friend model)
        {
            return new FriendViewModel
            {
                ID = model.ID,
                FriendUserName = model.FriendUser.UserName,
                FriendMainImagePath = model.FriendUser?.UserProfile?.MainImage,
                FriendUserID = model.FriendUserID
            
            };
        }
    }
}

