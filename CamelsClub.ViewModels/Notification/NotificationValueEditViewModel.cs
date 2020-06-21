using System.Runtime.Serialization;

namespace CamelsClub.ViewModels
{
    public class NotificationValueEditViewModel
    {
        public int ID { get; set; }
        public string Value { get; set; }
        public bool IsPicked { get; set; }
        
        [IgnoreDataMember]
        public bool IsDefault { get; set; }
    }
}