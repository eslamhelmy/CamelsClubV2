using System.Runtime.Serialization;

namespace CamelsClub.ViewModels
{
    public class CheckerBossApprovalCreateViewModel
    {
        [IgnoreDataMember]
        public int UserID { get; set; }
        public int CheckerApproveID { get; set; }
        //in case of termination he must put notes
        public string Notes { get; set; }
    }
}