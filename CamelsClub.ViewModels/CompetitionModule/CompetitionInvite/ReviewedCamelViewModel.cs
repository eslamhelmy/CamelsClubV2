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
    public class ReviewedCamelViewModel
    {
        public int ID { get; set; }
        public string CamelName { get; set; }
        public string CamelImagePath { get; set; }
        public double RefereePercentage { get; set; }
    }
}
