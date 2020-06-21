using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class FriendCreateViewModel
    {
        public int ID { get; set; }
        [IgnoreDataMember]
        public int UserID { get; set; }
        public int FriendUserID { get; set; }
        public string Notes { get; set; }
    }

    public static partial class FriendExtensions
    {
        public static Friend ToModel(this FriendCreateViewModel viewModel)
        {
            return new Friend
            {
                ID = viewModel.ID,
                FriendUserID = viewModel.FriendUserID,
                UserID = viewModel.UserID,
                Notes = viewModel.Notes

            };
        }
    }
}
