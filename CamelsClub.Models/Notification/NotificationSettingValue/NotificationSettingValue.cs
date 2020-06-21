
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
   
    public class NotificationSettingValue : BaseModel
    {
        public int NotificationSettingID { get; set; }
        public virtual NotificationSetting NotificationSetting { get; set; }
        public string TextArabic { get; set; }
        public string TextEnglish { get; set; }
        public bool IsDefault { get; set; }
        public virtual ICollection<UserNotificationSetting> Users { get; set; }

    }
}
