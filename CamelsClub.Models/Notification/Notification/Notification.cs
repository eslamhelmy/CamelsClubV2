using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
   
    public class Notification:BaseModel
    {
        
        public string ContentArabic { get; set; }
        public string ContentEnglish { get; set; }
        public int NotificationTypeID { get; set; }
        public virtual NotificationType NotificationType { get; set; }
        public DateTime? SeenDateTime { get; set; }
        public int SourceID { get; set; }
        public virtual User Source { get; set; }
        public int DestinationID { get; set; }
        public virtual User Destination { get; set; }
        public int? PostID { get; set; }
        public virtual Post Post { get; set; }
        public int? CommentID { get; set; }
        public virtual Comment Comment { get; set; }
        public int? MessageID { get; set; }
        public virtual Message Message { get; set; }

        public int? FriendRequestID { get; set; }
        public virtual FriendRequest FriendRequest { get; set; }
        public int? ActionID { get; set; }
        public virtual Action Action { get; set; }

        public int? CompetitionID { get; set; }
        public virtual Competition Competition { get; set; }

    }
}
