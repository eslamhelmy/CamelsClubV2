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
    public enum AssignTo
    {
        CheckerBoss = 1,
        Checker = 2,
        RefereeBoss = 3,
        Referee = 4
    }
    public class CompetitionTeamRewardCreateViewModel
    {
        public string TextArabic { get; set; }
        public string TextEnglish { get; set; }
      
        //[Required(ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        //[Range((int)(AssignTo.CheckerBoss), (int)(AssignTo.Referee), ErrorMessage = "invalid Assign to ")]
        public int AssignedTo { get; set; }
        [IgnoreDataMember]
        public int CompetitionID { get; set; }

    }

    public static partial class CompetitionTeamRewardExtensions
    {
        public static CompetitionTeamReward ToModel(this CompetitionTeamRewardCreateViewModel viewModel)
        {
            return new CompetitionTeamReward
            {
                TextArabic = viewModel.TextArabic,
                TextEnglish = viewModel.TextEnglish,
                CompetitionID = viewModel.CompetitionID,
                AssignedTo = viewModel.AssignedTo
            };
        }
    }
}
