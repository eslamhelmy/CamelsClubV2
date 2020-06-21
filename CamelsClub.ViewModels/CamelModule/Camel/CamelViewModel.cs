using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CamelViewModel
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public string MotherName { get; set; }
        public string FatherName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Location { get; set; }
        public string Details { get; set; }
        public string Code { get; set; }
        public int CategoryID { get; set; }
        public string CategoryArabicName { get; set; }
        public string CategoryEnglishName { get; set; }
        public int GenderID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string GenderName { get; set; }
        public List<CamelDocumentViewModel> camelDocuments { get; set; }
        public int CamelCompetitionID { get; set; }
        public string UserImage { get; set; }
    }
    public  static class CamelExtension
    {

        public static CamelViewModel ToViewModel(this Camel model)
        {
            return new CamelViewModel
            {
                ID = model.ID,
                Name = model.Name,
                MotherName = model.MotherName,
                FatherName = model.FatherName,
                BirthDate = model.BirthDate,
                CategoryID = model.CategoryID,
                Details = model.Details,
                Location = model.Location,
                UserName = model.User?.DisplayName,
                CategoryArabicName = model.Category?.NameArabic,
                CategoryEnglishName = model.Category?.NameEnglish,
                GenderID = model.GenderConfigDetailID,
                GenderName = model.GenderConfigDetail?.NameArabic
            };
        }
    }
}

