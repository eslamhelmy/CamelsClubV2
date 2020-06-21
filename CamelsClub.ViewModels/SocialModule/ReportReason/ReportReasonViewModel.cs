using CamelsClub.Models;

namespace CamelsClub.ViewModels
{
    public class ReportReasonViewModel
    {
        public int ID { get; set; }
        public string TextArabic { get; set; }
        public string TextEnglish { get; set; }
    }
    public  static partial class ReportReasonExtension
    {

        public static ReportReasonViewModel ToViewModel(this ReportReason model)
        {
            return new ReportReasonViewModel
            {
                ID = model.ID,
                TextArabic = model.TextArabic ,
                TextEnglish = model.TextEnglish
            };
        }
    }
}

