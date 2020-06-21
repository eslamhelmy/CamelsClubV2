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
    public class CompetitionSpecificationEditViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int ID { get; set; }
        public int CompetitionID { get; set; }
        public int CamelSpecificationID { get; set; }
        public decimal MaxAllowedValue { get; set; }
    }

    public static partial class CompetitionRewardExtensions
    {
        public static CompetitionSpecification ToModel(this CompetitionSpecificationEditViewModel viewModel)
        {
            return new CompetitionSpecification
            {
                ID = viewModel.ID,
                CompetitionID = viewModel.CompetitionID,
                MaxAllowedValue = viewModel.MaxAllowedValue,
                CamelSpecificationID = viewModel.CamelSpecificationID
            };
        }
    }
}
