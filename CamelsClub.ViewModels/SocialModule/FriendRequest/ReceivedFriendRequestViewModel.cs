using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class ReceivedFriendRequestViewModel
    {
        public int ID { get; set; }
        public string FromUserMainImagePath { get; set; }
        public string FromUserName { get; set; }
        public int FromUserID { get; set; }
        public string Notes { get; set; }
        public int Status { get; set; }
        public string FromDisplayName { get; set; }
    }
}

