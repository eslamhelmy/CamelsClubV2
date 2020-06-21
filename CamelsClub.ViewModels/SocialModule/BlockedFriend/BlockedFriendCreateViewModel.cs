using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class BlockedFriendCreateViewModel
    {
        public int ID { get; set; }
        [IgnoreDataMember]
        public int UserID { get; set; }
        public int BlockedFriendID { get; set; }
    }

    public static partial class FriendExtensions
    {
        public static BlockedFriend ToModel(this BlockedFriendCreateViewModel viewModel)
        {
            return new BlockedFriend
            {
                ID = viewModel.ID,
                BlockedFriendID = viewModel.BlockedFriendID,
                UserID = viewModel.UserID,
                
            };
        }
    }
}
