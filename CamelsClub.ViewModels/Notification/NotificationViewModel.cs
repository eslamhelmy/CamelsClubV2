using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class NotificationViewModel
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public bool IsSeen { get; set; }
        public int NotificationTypeID { get; set; }
        public string NotificationType { get; set; }
        public int SourceID { get; set; }
        public string SourceName { get; set; }
        public int DestinationID { get; set; }
        public string DestinationName { get; set; }
        public int? CompetitionID { get; set; }
        public int? CommentID { get; set; }
        public int? PostID { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool HasJoinedCompetition { get; set; }
        public bool IsBossChecker { get; set; }
        public bool IsChecker { get; set; }
        public bool IsBossReferee { get; set; }
        public bool IsReferee { get; set; }
        public bool IsInvitedUser { get; set; }
        public string SourceUserImage { get; set; }
    }
}

