using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CompetitionTeamRewardViewModel
    {
        public int ID { get; set; }
        public string TextArabic { get; set; }
        public string TextEnglish { get; set; }
        public string CompetitionNameArabic { get; set; }
        public string CompetitionNameEnglish { get; set; }
        public int CompetitionID { get; set; }
        public int AssignedTo { get; set; }

    }
    public  static class CompetitionTeamRewardExtension
    {

        public static CompetitionTeamRewardViewModel ToViewModel(this CompetitionTeamReward model)
        {
            return new CompetitionTeamRewardViewModel
            {
                ID = model.ID,
                TextArabic = model.TextArabic,
                TextEnglish = model.TextEnglish,
                CompetitionID = model.CompetitionID,
                CompetitionNameArabic =model.Competition?.NameArabic,
                CompetitionNameEnglish =model.Competition?.NamEnglish,
               AssignedTo = model.AssignedTo
            };
        }
    }
}

