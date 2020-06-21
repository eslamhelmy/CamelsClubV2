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
    public class CompetitionSpecificationCreateViewModel
    {
        [IgnoreDataMember]
        public int CompetitionID { get; set; }
        public int CamelSpecificationID { get; set; }
        public decimal MaxAllowedValue { get; set; }
    }

    public static partial class CompetitionSpecificationExtensions
    {
        public static CompetitionSpecification ToModel(this CompetitionSpecificationCreateViewModel viewModel)
        {
            return new CompetitionSpecification
            {
                CompetitionID = viewModel.CompetitionID,
                MaxAllowedValue = viewModel.MaxAllowedValue,
                CamelSpecificationID = viewModel.CamelSpecificationID
            };
        }
    }
}
