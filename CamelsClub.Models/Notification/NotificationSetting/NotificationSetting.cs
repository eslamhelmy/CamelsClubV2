
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
   
    public class NotificationSetting : BaseModel
    {
        public string TextArabic { get; set; }
        public string TextEnglish { get; set; }

        public virtual ICollection<UserNotificationSetting> UserNotificationSettings { get; set; }
        public virtual ICollection<NotificationSettingValue> Values { get; set; }
    }
}
