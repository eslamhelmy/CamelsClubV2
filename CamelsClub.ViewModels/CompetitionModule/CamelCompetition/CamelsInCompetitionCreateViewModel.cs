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
    public class JoinCompetitionCreateViewModel
    {
        [IgnoreDataMember]
        public int UserID { get; set; }
        public int GroupID { get; set; }
        public int CompetitionID { get; set; }
      
    }

    public class SendCompetitionResultCreateViewModel
    {
        [IgnoreDataMember]
        public int UserID { get; set; }
        public int CompetitionID { get; set; }

    }

}
