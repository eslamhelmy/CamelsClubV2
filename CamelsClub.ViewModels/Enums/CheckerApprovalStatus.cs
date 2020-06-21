using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels.Enums
{
    public enum CheckerApprovalStatus
    {
        FinishedBySubChecker = 1,
        ApprovedByBoss = 2 , 
        RejectedByBoss = 3,
        ReplacedByUser = 4,
        Terminate = 5
    }
}
