using CamelsClub.ViewModels;
using System.Collections.Generic;

namespace CamelsClub.Services
{
    public class RefereesReportViewModel
    {
        public int TotalNumberOfAllCamels { get; set; }
        public int TotalNumberOfEvaluatedCamels { get; set; }
        public int CheckingCompletitionRatio { get; set; }
        public List<CompetitionRefereeViewModel> Referees { get; set; }
    }
}