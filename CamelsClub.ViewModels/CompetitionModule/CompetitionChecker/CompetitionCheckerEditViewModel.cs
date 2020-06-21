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
    public class CompetitionCheckerEditViewModel
    {
        [Required]
        public int ID { get; set; }
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int UserID { get; set; }
        public int CompetitionID { get; set; }
        public bool IsBoss { get; set; }

    }

    public static partial class CompetitionCheckerExtensions
    {
        public static CompetitionChecker ToModel(this CompetitionCheckerEditViewModel viewModel)
        {
            return new CompetitionChecker
            {
                ID = viewModel.ID,
                UserID = viewModel.UserID,
                CompetitionID = viewModel.CompetitionID,
                IsBoss = viewModel.IsBoss
                
            };
        }
    }
}
