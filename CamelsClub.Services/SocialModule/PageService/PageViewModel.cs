using System.Collections.Generic;

namespace CamelsClub.Services
{
    public class PageViewModel
    {
        public int ID { get; set; }
        public string NameArabic { get; set; }
        public string NameEnglish { get; set; }
        public List<PermissionViewModel> Permissions { get; set; }
    }
}