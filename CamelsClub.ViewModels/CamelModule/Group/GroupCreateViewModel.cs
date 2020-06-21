using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class GroupCreateViewModel
    {
        public int ID { get; set; }
        public string NameArabic { get; set; }
        public string NameEnglish { get; set; }
        [IgnoreDataMember]
        public int UserID { get; set; }
        public string Image { get; set; }
        public int CategoryID { get; set; }
        public List<CamelGroupCreateViewModel> Camels { get; set; }

    }

    public static partial class GroupExtensions
    {
        public static Group ToModel(this GroupCreateViewModel viewModel)
        {
            return new Group
            {
                ID = viewModel.ID,
                Image = viewModel.Image,
                NameArabic = viewModel.NameArabic,
                NameEnglish = viewModel.NameEnglish,
                UserID = viewModel.UserID ,
                CategoryID = viewModel.CategoryID
            };
        }
    }
}
