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
    public class RefereeAllocateCreateViewModel
    {
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int GroupID { get; set; }
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int RefereeID { get; set; }
    }

    public static partial class ManualAllocateExtensions
    {
        public static CompetitionRefereeAllocate ToModel(this RefereeAllocateCreateViewModel viewModel)
        {
            return new CompetitionRefereeAllocate
            {
                GroupID = viewModel.GroupID,
                CompetitionRefereeID = viewModel.RefereeID,
                
            };
        }
    }
}
