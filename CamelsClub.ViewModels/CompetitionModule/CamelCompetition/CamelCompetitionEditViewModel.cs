using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CamelCompetitionEditViewModel
    {
        [Required]
        public int ID { get; set; }
        [Range(1, int.MaxValue,ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int CamelID { get; set; }
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int CompetitionID { get; set; }

        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int CompetitionInviteID { get; set; }

    }

    public static partial class CamelCompetitionExtensions
    {
        public static CamelCompetition ToModel(this CamelCompetitionEditViewModel viewModel)
        {
            return new CamelCompetition
            {
                ID = viewModel.ID,
                CamelID = viewModel.CamelID ,
                CompetitionID = viewModel.CompetitionID,
                CompetitionInviteID = viewModel.CompetitionInviteID
            };
        }
    }
}
