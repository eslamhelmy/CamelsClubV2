using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class UserChatViewModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string LastMessage { get; set; }
        [IgnoreDataMember]
        public int LastMessageID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserImage { get; set; }
        public int UnSeenMessagesCount { get; set; }
        public string DisplayName { get; set; }
    }
}
