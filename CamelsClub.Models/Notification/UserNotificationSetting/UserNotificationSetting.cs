using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
   
    public class UserNotificationSetting : BaseModel
    {
        
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public int NotificationSettingID { get; set; }
        public virtual NotificationSetting NotificationSetting { get; set; }
        public int NotificationSettingValueID { get; set; }
        public virtual NotificationSettingValue NotificationSettingValue { get; set; }
    }
}
