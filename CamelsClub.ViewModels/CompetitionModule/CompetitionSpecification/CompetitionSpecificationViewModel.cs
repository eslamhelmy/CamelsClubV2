using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CompetitionSpecificationViewModel
    {
        public int ID { get; set; }
        public decimal MaxAllowedValue { get; set; }
        public int CamelSpecificationID { get; set; }
        public string SpecificationNameArabic { get; set; }
        public string SpecificationNameEnglish { get; set; }
       
    }
    public  static class CompetitionSpecificationExtension
    {

        public static CompetitionSpecificationViewModel ToViewModel(this CompetitionSpecification model)
        {
            return new CompetitionSpecificationViewModel
            {
                ID = model.ID,
                MaxAllowedValue = model.MaxAllowedValue,
                CamelSpecificationID = model.CamelSpecificationID
              
            };
        }
    }
}

