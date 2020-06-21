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
    public class IssueReportCreateViewModel
    {

     
        public int ID { get; set; }
        [Required(ErrorMessageResourceType = typeof(Localization.Shared.Resource), ErrorMessageResourceName = "RequiredFieldValidation")]
        public int PostID { get; set; }
        [IgnoreDataMember]
        public int UserID { get; set; }
        public string Text { get; set; }
        public int ReportReasonID { get; set; }

    }
    public static class IssueReportExtension
    {

        public static IssueReport ToModel(this IssueReportCreateViewModel viewmodel)
        {
            return new IssueReport
            {
                ID = viewmodel.ID,
                PostID = viewmodel.PostID,
                UserID = viewmodel.UserID,
                ReportReasonID = viewmodel.ReportReasonID,
                Text = viewmodel.Text
            };
        }

    }
}

