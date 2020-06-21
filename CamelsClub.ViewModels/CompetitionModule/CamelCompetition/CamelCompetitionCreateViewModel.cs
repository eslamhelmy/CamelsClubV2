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
    public class CamelCompetitionCreateViewModel
    {
      //  [Range(1, int.MaxValue,ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int CamelID { get; set; }
      //  [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int CompetitionID { get; set; }
        [IgnoreDataMember]
        public int CompetitionInviteID { get; set; }
        
    }

    public static partial class CamelCompetitionExtensions
    {
        public static CamelCompetition ToModel(this CamelCompetitionCreateViewModel viewModel)
        {
            return new CamelCompetition
            {
                CamelID = viewModel.CamelID ,
                CompetitionID = viewModel.CompetitionID,
                CompetitionInviteID = viewModel.CompetitionInviteID
            };
        }
    }
}
