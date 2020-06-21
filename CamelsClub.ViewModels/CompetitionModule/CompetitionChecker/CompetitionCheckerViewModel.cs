using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CompetitionCheckerViewModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public bool IsBoss { get; set; }
        public bool HasJoined { get; set; }
        public bool HasPicked { get; set; }
        public bool HasFinisedRating { get; set; }
        public int CamelsEvaluatedCount { get; set; }
        public int AssignedCamels { get; set; }
        public int CompletionRatio { get; set; }
        public int CamelsIApproved { get; set; }
        public int TotalCamelsCount { get; set; }
        public int AssignedGroups { get; set; }
        public string CheckerImage { get; set; }
        public string DisplayName { get; set; }
    }
    public  static class CompetitionCheckerExtension
    {

        public static CompetitionCheckerViewModel ToViewModel(this CompetitionChecker model)
        {
            return new CompetitionCheckerViewModel
            {
                ID = model.ID,
                UserName = model.User.UserName,
             //   CompetitionID = model.CompetitionID
              
            };
        }
    }
}

