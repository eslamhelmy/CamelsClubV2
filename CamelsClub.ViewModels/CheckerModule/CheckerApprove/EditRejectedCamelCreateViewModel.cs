using System.Runtime.Serialization;

namespace CamelsClub.ViewModels
{
    public class EditRejectedCamelCreateViewModel
    {
        public int CheckerApproveID { get; set; }
        [IgnoreDataMember]
        public int LoggedUserID { get; set; }
    }
}