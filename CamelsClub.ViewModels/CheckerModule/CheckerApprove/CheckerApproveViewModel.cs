using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CheckerApproveViewModel
    {
      
        public int ID { get; set; }
        public int CamelCompetitionID { get; set; }
        public string CamelName { get; set; }
        public string SubCheckerName { get; set; }
        public string CompetitionName { get; set; }
        public string Notes { get; set; }
        public int Status { get; set; }
        public int CamelID { get; set; }
        public string CompetitionImagePath { get; set; }
        public List<CamelDocumentViewModel> CamelImages { get; set; }
        public List<ReviewApproveViewModel> Reviews { get; set; }
        public List<ReviewApproveViewModel> ReviewRequests { get; set; }
        public string CheckerImage { get; set; }
    }
    
}

