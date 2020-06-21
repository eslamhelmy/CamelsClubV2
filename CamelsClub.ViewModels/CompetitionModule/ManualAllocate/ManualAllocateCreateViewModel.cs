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
    public class ManualAllocateCreateViewModel
    {
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int GroupID { get; set; }
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int CheckerID { get; set; }
    }

    public static partial class ManualAllocateExtensions
    {
        public static CompetitionAllocate ToModel(this ManualAllocateCreateViewModel viewModel)
        {
            return new CompetitionAllocate
            {
                GroupID = viewModel.GroupID,
                CompetitionCheckerID = viewModel.CheckerID,
                
            };
        }
    }
}
