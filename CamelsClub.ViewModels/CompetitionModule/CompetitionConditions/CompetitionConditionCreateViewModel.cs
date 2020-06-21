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
    public class CompetitionConditionCreateViewModel
    {
     //   [Required(ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public string TextArabic { get; set; }
        public string TextEnglish { get; set; }

       [IgnoreDataMember]
        public int CompetitionID { get; set; }
     
    }

    public static partial class CompetitionConditionExtensions
    {
        public static CompetitionCondition ToModel(this CompetitionConditionCreateViewModel viewModel)
        {
            return new CompetitionCondition
            {
                TextArabic = viewModel.TextArabic,
                TextEnglish = viewModel.TextEnglish,
                CompetitionID = viewModel.CompetitionID
                
            };
        }
    }
}
