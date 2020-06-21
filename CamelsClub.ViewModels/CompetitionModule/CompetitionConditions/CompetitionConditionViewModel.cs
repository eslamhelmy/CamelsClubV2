using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CompetitionConditionViewModel
    {
        public int ID { get; set; }
        public string TextArabic { get; set; }
        public string TextEnglish { get; set; }
        public string CompetitionNameArabic { get; set; }
        public string CompetitionNameEnglish { get; set; }
        public int CompetitionID { get; set; }
     }
    public  static class CompetitionConditionExtension
    {

        public static CompetitionConditionViewModel ToViewModel(this CompetitionCondition model)
        {
            return new CompetitionConditionViewModel
            {
                ID = model.ID,
                TextArabic = model.TextArabic,
                TextEnglish = model.TextEnglish,
                CompetitionID = model.CompetitionID,
                CompetitionNameArabic =model.Competition.NameArabic,
                CompetitionNameEnglish =model.Competition.NamEnglish,
              
            };
        }
    }
}

