using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class UserProfileViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string MainImagePath { get; set; }
        public string CoverImagePath { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Gender { get; set; }
        public string Address { get; set; }
        public int NumberOfCompetitionsJoined { get; set; }
        public int NumberOfFriends { get; set; }
        public int NumberOfRefereesJoined { get; set; }
        
        public IEnumerable<ProfileImageViewModel> ProfileImages { get; set; }
        public IEnumerable<PostDetailsViewModel> Posts { get; set; }
        public IEnumerable<ProfileVideoViewModel> ProfileVideos { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public List<CamelViewModel> Camels { get; set; }
        public int GroupsCount { get; set; }
        public int CamelsCount { get; set; }
        public bool HasSentFriendReuestToMine { get; set; }
        public bool HasReceivedFriendRequestFromMine { get; set; }
        public bool IsFriend { get; set; }
        public bool IsBlocked { get; set; }
    }
}
