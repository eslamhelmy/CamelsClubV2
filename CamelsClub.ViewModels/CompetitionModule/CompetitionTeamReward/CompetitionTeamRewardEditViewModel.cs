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
    public class CompetitionTeamRewardEditViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int ID { get; set; }
        [Required(ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public string TextArabic { get; set; }
        public string TextEnglish { get; set; }
        public int AssignedTo { get; set; }

        [Required(ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int CompetitionID { get; set; }
 
    }

    public static partial class CompetitionTeamRewardExtensions
    {
        public static CompetitionTeamReward ToModel(this CompetitionTeamRewardEditViewModel viewModel)
        {
            return new CompetitionTeamReward
            {
                ID = viewModel.ID,
                TextArabic = viewModel.TextArabic,
                TextEnglish = viewModel.TextEnglish,
                CompetitionID = viewModel.CompetitionID,
                AssignedTo = viewModel.AssignedTo
                
            };
        }
    }
}
