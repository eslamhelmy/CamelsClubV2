using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class GroupViewModel
    {
      
        public int ID { get; set; }
        public string NameArabic { get; set; }
        public string NameEnglish { get; set; }
        public string UserName { get; set; }
        public string CategoryNameArabic { get; set; }
        public string CategoryNameEnglish { get; set; }
        public string  ImagePath { get; set; }
        public decimal RefereeCompletionPercentage { get; set; }
        public List<CamelGroupViewModel> camelsGroup { get; set; }
        public List<CompetitionCheckerViewModel> AssignedCheckers { get; set; }
        public int CamelsCountInGroup { get; set; }
        public List<CompetitionRefereeViewModel> AssignedReferees { get; set; }
        public bool IsGroupApproved { get; set; }
        public bool IsGroupRejected { get; set; }
        public bool IsCheckerFinishedRating { get; set; }
        public bool IsRefereeFinishedRating { get; set; }
    }
    public  static class GroupExtension
    {

        public static GroupViewModel ToViewModel(this Group model)
        {
            return new GroupViewModel
            {
                ID = model.ID,
                ImagePath = model.Image,
                NameArabic = model.NameArabic ,
                NameEnglish = model.NameEnglish ,
                UserName = model.User?.UserName ,
                CategoryNameArabic = model.Category?.NameArabic,
                CategoryNameEnglish = model.Category?.NameEnglish
            };
        }
    }
}

