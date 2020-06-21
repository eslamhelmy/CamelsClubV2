using System.Runtime.Serialization;

namespace CamelsClub.ViewModels
{
    public class ReplaceRejectedCamelCreateViewModel
    {
        public int CheckerApproveID { get; set; }
        public int CamelID { get; set; }
        [IgnoreDataMember]
        public int LoggedUserID { get; set; }
    }
}