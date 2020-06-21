using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class BlockedFriendViewModel
    {
        public int ID { get; set; }
        public string BlockedFriendUserName { get; set; }
        public string BlockedFriendMainImagePath { get; set; }
        public int BlockedFriendID { get; set; }
      
    }
    public  static class BlockedFriendExtension
    {

        public static BlockedFriendViewModel ToViewModel(this BlockedFriend model)
        {
            return new BlockedFriendViewModel
            {
                ID = model.ID,
                BlockedFriendMainImagePath = model.BlockFriend?.UserProfile?.MainImage,
                BlockedFriendUserName = model.BlockFriend?.UserName,
                BlockedFriendID = model.BlockedFriendID
            
            };
        }
    }
}

