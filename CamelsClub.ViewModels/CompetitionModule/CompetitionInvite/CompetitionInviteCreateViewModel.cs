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
    public class CompetitionInviteCreateViewModel
    {
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int UserID { get; set; }
       [IgnoreDataMember]
        public int CompetitionID { get; set; }
     
    }

    public static partial class CompetitionInviteExtensions
    {
        public static CompetitionInvite ToModel(this CompetitionInviteCreateViewModel viewModel)
        {
            return new CompetitionInvite
            {
                UserID = viewModel.UserID,
                CompetitionID = viewModel.CompetitionID
                
            };
        }
    }
}
