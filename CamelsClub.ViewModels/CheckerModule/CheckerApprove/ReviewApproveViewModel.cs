using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class ReviewApproveViewModel
    {
        public int ID { get; set; }
        public int CheckerID { get; set; }
        public string CheckerName { get; set; }
        public string CheckerImage { get; set; }
        public int CheckerApproveID { get; set; }
        public string OldNotes { get; set; }
        public string NewNotes { get; set; }
   
    }

}

