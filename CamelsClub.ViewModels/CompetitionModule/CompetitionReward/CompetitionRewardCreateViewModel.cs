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
    public enum Rank
    {
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4,
        Fifth = 5
    }
    public class CompetitionRewardCreateViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public string NameArabic { get; set; }
        public string NamEnglish { get; set; }
        public string SponsorText { get; set; }
        public string Logo { get; set; }
        [IgnoreDataMember]
        public int CompetitionID { get; set; }
        public int? SponsorID { get; set; }
        [Required(ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        [Range((int)(Rank.First), (int)(Rank.Fifth), ErrorMessage = "invalid Assign to ")]
        public Rank Rank { get; set; }
    }

    public static partial class CompetitionRewardExtensions
    {
        public static CompetitionReward ToModel(this CompetitionRewardCreateViewModel viewModel)
        {
            return new CompetitionReward
            {
                NameArabic = viewModel.NameArabic,
                NameEnglish = viewModel.NamEnglish,
                CompetitionID = viewModel.CompetitionID,
                SponsorID = viewModel.SponsorID,
                SponsorText = viewModel.SponsorText,
                Logo = viewModel.Logo,
                Rank = (int) viewModel.Rank
            };
        }
    }
}
