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
    public class ReviewedCompetitorViewModel
    {
        public int CompetitorID { get; set; }
        public string CompetitorName { get; set; }
        public string CompetitorImagePath { get; set; }
        public double RefereePercentage { get; set; }
    }
}
