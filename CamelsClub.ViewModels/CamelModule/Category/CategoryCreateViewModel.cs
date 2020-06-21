using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CategoryCreateViewModel
    {
        public int ID { get; set; }
        public string NameArabic { get; set; }
        public string NameEnglish { get; set; }
     
    }

    public static partial class CategoryExtensions
    {
        public static Category ToModel(this CategoryCreateViewModel viewModel)
        {
            return new Category
            {
                ID = viewModel.ID,
                NameArabic = viewModel.NameArabic,
                NameEnglish = viewModel.NameEnglish
            };
        }
    }
}
