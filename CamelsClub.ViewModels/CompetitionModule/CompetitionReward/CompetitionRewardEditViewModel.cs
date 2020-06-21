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
    public class CompetitionRewardEditViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int ID { get; set; }
        [Required(ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public string NameArabic { get; set; }
        public string NamEnglish { get; set; }
        public string SponsorText { get; set; }

        [Required(ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int CompetitionID { get; set; }
        public int? SponsorID { get; set; }

    }

    public static partial class CompetitionRewardExtensions
    {
        public static CompetitionReward ToModel(this CompetitionRewardEditViewModel viewModel)
        {
            return new CompetitionReward
            {
                ID = viewModel.ID,
                NameArabic = viewModel.NameArabic,
                NameEnglish = viewModel.NamEnglish,
                CompetitionID = viewModel.CompetitionID,
                SponsorID = viewModel.SponsorID
                
            };
        }
    }
}
