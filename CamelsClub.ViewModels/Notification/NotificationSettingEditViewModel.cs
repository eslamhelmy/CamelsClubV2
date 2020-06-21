using System.Collections.Generic;

namespace CamelsClub.ViewModels
{
    public class NotificationSettingEditViewModel
    {
        public int ID { get; set; }
        public string Question { get; set; }
        public List<NotificationValueEditViewModel> Values { get; set; }
    }
}