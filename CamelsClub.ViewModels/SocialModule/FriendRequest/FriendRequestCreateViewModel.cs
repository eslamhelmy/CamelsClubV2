using CamelsClub.Models;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class FriendRequestCreateViewModel
    {
        public int ID { get; set; }
        [IgnoreDataMember]
        public int FromUserID { get; set; }
        public int ToUserID { get; set; }
        public int Status { get; set; }
        public string Notes { get; set; }
    }

    public static partial class CamelExtensions
    {
        public static FriendRequest ToModel(this FriendRequestCreateViewModel viewModel)
        {
            return new FriendRequest
            {
                ID = viewModel.ID,
                FromUserID = viewModel.FromUserID,
                ToUserID = viewModel.ToUserID,
                Status = (int)FriendRequestStatus.Pending,
                Notes = viewModel.Notes

            };
        }
    }
}
