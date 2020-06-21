using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class UserNotificationSettingEditViewModel
    {
        public int ID { get; set; }
        public int NotificationSettingID { get; set; }
        public int NotificationSettingValueID { get; set; }
        public string NotificationSettingText { get; set; }
        [IgnoreDataMember]
        public int UserID { get; set; }
    }
}

