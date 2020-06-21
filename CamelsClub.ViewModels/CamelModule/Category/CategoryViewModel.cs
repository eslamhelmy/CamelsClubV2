using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CategoryViewModel
    {
      
        public int ID { get; set; }
        public string NameArabic { get; set; }
        public string NameEnglish { get; set; }
    }
    public  static class CategoryExtension
    {

        public static CategoryViewModel ToViewModel(this Category model)
        {
            return new CategoryViewModel
            {
                ID = model.ID,
                NameArabic = model.NameArabic ,
                NameEnglish = model.NameEnglish
            };
        }
    }
}

