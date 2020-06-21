using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels.Enums
{
    public enum InvitedUserStatus
    {
        ApprovedBySubChecker = 1,
        PartialApprovalBySubChecker = 2 , 
        AllCamelsNotCheckedYet = 3 ,
        RejectedTillUpdateFromUser = 4 ,
        UpdatedByUserAndWaitingForApprovalAgain = 5

    }
}
