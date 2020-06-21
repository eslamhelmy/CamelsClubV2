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
    public class CheckerJoinCompetitionCreateViewModel
    {
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int CompetitionID { get; set; }
        [IgnoreDataMember]
        public int UserID { get; set; }

    }

  
}
