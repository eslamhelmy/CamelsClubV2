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
    public class CheckerApproveCreateViewModel
    {
        //camel competition Id
        [Required]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int ID { get; set; }
        public string Notes { get; set; }
        [IgnoreDataMember]
        public int UserID { get; set; }
        [IgnoreDataMember]
        public int CompetitionCheckerID { get; set; }
        [Required]
        public CheckerApprovalStatus Status { get; set; }
    }

    public enum CheckerApprovalStatus
    {
        Approve = 1,
        Reject = 2
    }
    public static partial class CheckerApproveExtensions
    {
        public static CheckerApprove ToModel(this CheckerApproveCreateViewModel viewModel)
        {
            return new CheckerApprove
            {
             //   ID = viewModel.ID,
                CompetitionCheckerID = viewModel.CompetitionCheckerID,
                CamelCompetitionID = viewModel.ID ,
                Notes = viewModel.Notes ,
                Status = (int) viewModel.Status
            };
        }
    }
}
