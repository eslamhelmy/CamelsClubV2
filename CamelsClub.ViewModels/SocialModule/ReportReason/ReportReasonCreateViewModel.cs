using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class ReportReasonCreateViewModel
    {
        public int ID { get; set; }
        public string TextArabic { get; set; }
        public string TextEnglish { get; set; }
     
    }

    public static partial class ReportReasonExtensions
    {
        public static ReportReason ToModel(this ReportReasonCreateViewModel viewModel)
        {
            return new ReportReason
            {
                ID = viewModel.ID,
                TextArabic = viewModel.TextArabic,
                TextEnglish = viewModel.TextEnglish
            };
        }
    }
}
