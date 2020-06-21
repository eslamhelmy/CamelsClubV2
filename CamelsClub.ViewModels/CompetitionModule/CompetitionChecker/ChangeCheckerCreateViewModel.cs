using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class ChangeCheckerCreateViewModel
    {
        [IgnoreDataMember]
        public int UserID { get; set; }
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int OldCheckerID { get; set; }
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int NewCheckerID { get; set; }
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int CompetitionID { get; set; }
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int GroupID { get; set; }

    }


    public class ChangeRefereeCreateViewModel
    {
        [IgnoreDataMember]
        public int UserID { get; set; }
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int OldRefereeID { get; set; }
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int NewRefereeID { get; set; }
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int CompetitionID { get; set; }
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int GroupID { get; set; }

    }


}
