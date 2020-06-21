

using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CompetitionRefereeViewModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public bool IsBoss { get; set; }
        public List<CamelSpecificationViewModel> Vaules { get; set; }
        public bool HasEvaluated { get; set; }
        public bool HasJoined { get; set; }
        public bool HasPicked { get; set; }
        public int CamelsEvaluatedCount { get; set; }
        public int AssignedCamels { get; set; }
        public int TotalCamelsCount { get; set; }
        public int AssignedGroups { get; set; }
        public int CompletionRatio { get; set; }
        public string DisplayName { get; set; }
    }
    public static class CompetitionRefereeExtension
    {

        public static CompetitionRefereeViewModel ToViewModel(this CompetitionReferee model)
        {
            return new CompetitionRefereeViewModel
            {
                ID = model.ID,
                UserName = model.User.UserName,
      //          CompetitionID = model.CompetitionID

            };
        }
    }
}
