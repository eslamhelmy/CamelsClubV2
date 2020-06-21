using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CompetitionRewardViewModel
    {
        public int ID { get; set; }
        public string NameArabic { get; set; }
        public string NamEnglish { get; set; }
        public string CompetitionNameArabic { get; set; }
        public string CompetitionNameEnglish { get; set; }
        public int CompetitionID { get; set; }
        public string SponsorText { get; set; }
        public Rank Rank { get; set; }

    }
    public  static class CompetitionRewardExtension
    {

        public static CompetitionRewardViewModel ToViewModel(this CompetitionReward model)
        {
            return new CompetitionRewardViewModel
            {
                ID = model.ID,
                NameArabic = model.NameArabic,
                NamEnglish = model.NameEnglish,
                CompetitionID = model.CompetitionID,
                CompetitionNameArabic =model.Competition.NameArabic,
                CompetitionNameEnglish =model.Competition.NamEnglish,
              
            };
        }
    }
}

