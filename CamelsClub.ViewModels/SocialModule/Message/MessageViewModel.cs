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
    public class MessageViewModel
    {
        public int ID { get; set; }
      //  public int ToUserID { get; set; }
      //  public string ToUserName { get; set; }
      //  public string ToUserMainImagePath { get; set; }
        public string Text { get; set; }
        public bool Received { get; set; }
       [IgnoreDataMember]
        public DateTime CreatedDate { get; set; }
        public string FormattedDateTime { get; set; }
    }
   
}

