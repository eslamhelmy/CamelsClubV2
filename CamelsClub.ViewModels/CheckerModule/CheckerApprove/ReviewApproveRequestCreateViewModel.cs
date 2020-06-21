using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class ReviewApproveRequestCreateViewModel
    {
        public int CheckerID { get; set; }
        public int CheckerApproveID { get; set; }
    }

}

