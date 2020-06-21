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
    public class CamelCompetitionViewModel
    {
      
        public int ID { get; set; }
        public int CamelID { get; set; }
        public int? CheckerApproveID { get; set; }
        public string CheckerNotes { get; set; }
        public string CamelName { get; set; }
        public string CategoryArabicName { get; set; }
        public string CategoryEnglishName { get; set; }
        public bool IsEvaluated { get; set; }
      //  public bool IsTerminatedByBoss { get; set; }
        public List<CamelDocumentViewModel> CamelImages { get; set; }
        public string CompetitionName { get; set; }
        [IgnoreDataMember]
        public string UserName { get; set; }
        [IgnoreDataMember]
        public string UserImage { get; set; }
        public int CompetitionID { get; set; }
        public int EvaluateStatus { get; set; }
        public decimal CompletionPercentage { get; set; }
        public List<CheckerApproveViewModel> CheckerEvaluates { get; set; }
        public List<CompetitionCheckerViewModel> NotEvaluatedByCheckers { get; set; }
        public IEnumerable<CompetitionRefereeViewModel> RefereeEvaluates { get; set; }
        public List<CompetitionRefereeViewModel> NotEvaluatedByReferees { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
        public DateTime? RejectedDateTime { get; set; }
        public List<CamelSpecificationViewModel> AverageSpecification { get; set; }
        public List<CamelSpecificationViewModel> Values { get; set; }
        public int Total { get; set; }
        [IgnoreDataMember]
        public int GroupID { get; set; }
        [IgnoreDataMember]
        public int CompetitorID { get; set; }
        [IgnoreDataMember]
        public string GroupName { get; set; }
        [IgnoreDataMember]
        public string GroupImage { get; set; }
    }
    public  static class CamelCompetitionExtension
    {

        public static CamelCompetitionViewModel ToViewModel(this CamelCompetition model)
        {
            return new CamelCompetitionViewModel
            {
                ID = model.ID,
                CamelID = model.CamelID,
                CompetitionID = model.CompetitionID,
               
            };
        }
    }
}

