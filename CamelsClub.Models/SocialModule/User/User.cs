using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class User : BaseModel
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string NID { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    //it throws another comment_id
        //    public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<PostUserAction> PostUserActions { get; set; }
        public virtual ICollection<CommentUserAction> CommentUserActions { get; set; }

        public virtual ICollection<Camel> Camels { get; set; }
        public virtual ICollection<Group> Groups { get; set; }


        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserConfirmationMessage> UserConfirmationMessages { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
        public virtual ICollection<FriendRequest> FromFriendRequests { get; set; }
        public virtual ICollection<FriendRequest> ToFriendRequests { get; set; }
        public virtual ICollection<Friend> Friends { get; set; }
        public virtual ICollection<Friend> FriendUsers { get; set; }
        public virtual ICollection<Competition> Competitions { get; set; }
        public virtual ICollection<CompetitionInvite> CompetitionInvites { get; set; }
        public virtual ICollection<CompetitionReferee> CompetitionReferees { get; set; }
        //public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<CompetitionChecker> CompetitionCheckers { get; set; }
        public virtual ICollection<IssueReport> IssueReports { get; set; }
        public virtual ICollection<BlockedFriend> BlockedFriends { get; set; }
        public virtual ICollection<BlockedFriend> FriendsBlockedMe { get; set; }
        public virtual ICollection<Message> SentMessages { get; set; }
        public virtual ICollection<Message> ReceivedMessages { get; set; }

        // public virtual ICollection<CheckerApprove> InvitesIApproved { get; set; }
        public virtual ICollection<UserNotificationSetting> UserNotificationSettings { get; set; }


    }
}
