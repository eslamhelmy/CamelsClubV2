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
    public class CompetitionInviteViewModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string UserImage { get; set; }
        public int UserStatus { get; set; }
        [IgnoreDataMember]
        public List<CheckerApproveViewModel> Approves { get; set; }
        [IgnoreDataMember]
        public int JoinedCamelsCount { get; set; }
        public bool HasJoined { get; set; }
        public double? FinalScore { get; set; }
    }
    public  static class CompetitionInviteExtension
    {

        public static CompetitionInviteViewModel ToViewModel(this CompetitionInvite model)
        {
            return new CompetitionInviteViewModel
            {
                ID = model.ID,
                UserName = model.User.UserName,
             //   CompetitionID = model.CompetitionID
              
            };
        }
    }
}

