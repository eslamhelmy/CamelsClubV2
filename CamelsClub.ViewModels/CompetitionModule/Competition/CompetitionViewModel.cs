using CamelsClub.Models;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CompetitionViewModel
    {
        public int ID { get; set; }
        public string NameArabic { get; set; }
        public string NamEnglish { get; set; }
        public string Address { get; set; }
        public int CamelsCount { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string CategoryArabicName { get; set; }
        public string CategoryEnglishName { get; set; }
        public int MinimumCheckersCount { get; set; }
        public int MinimumRefereesCount { get; set; }
        public int MinimumCompetitorsCount { get; set; }
        
        public int CategoryID { get; set; }
        public string UserName { get; set; }
        public string ImagePath { get; set; }
        public int CompetitionType { get; set; }
        public DateTime? Completed { get; set; }
        public DateTime? StartedDate { get; set; }
        public IEnumerable<CamelViewModel> Camels { get; set; }
        public IEnumerable<CompetitionWinnerViewModel> Winners { get; set; }
        public IEnumerable<CompetitionRewardViewModel> Rewards { get; set; }
        public IEnumerable<CompetitionInviteViewModel> Invites { get; set; }
        public IEnumerable<CompetitionRefereeViewModel> Referees { get; set; }
        public IEnumerable<CompetitionCheckerViewModel> Checkers { get; set; }
        public IEnumerable<CompetitionConditionViewModel> Conditions { get; set; }
        public IEnumerable<CompetitionSpecificationViewModel> Specifications { get; set; }
        public DateTime? Published { get; set; }
        [IgnoreDataMember]
        public int UserID { get; set; }
        public bool Created { get; set; }
        public bool HasJoined { get; set; }
        public bool HasPicked { get; set; }
        public bool HasRejected { get; set; }
        public bool IsChecker { get; set; }
        public bool IsCheckerBoss { get; set; }
        public bool IsReferee { get; set; }
        public bool IsBossReferee { get; set; }
        public bool IsCompetitor { get; set; }
        public List<CompetitionTeamRewardViewModel> TeamRewards { get; set; }
        public bool IsCheckersAllocated { get; set; }
        public bool IsRefereesAllocated { get; set; }
        public bool IsCheckerPickupTeam { get; set; }
        public bool IsRefereePickupTeam { get; set; }
        public double? CheckingCompletionPercentage { get; set; }
        public double? RefereeingCompletionPercentage { get; set; }
        public List<CamelCompetitionViewModel> CamelCompetitions { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ConditionText { get; set; }
        public ModuleComplateViewModel ModuleCompletion { get; set; }
        public bool ShowReferees { get; set; }
        public bool ShowCompetitors { get; set; }
        public bool ShowCheckers { get; set; }
    }

    public class ModuleComplateViewModel
    {
        public bool CheckingModuleDone { get; set; }
        public bool RefereeModuleDone { get; set; }
        public bool InviteCompetitorsModuleDone { get; set; }
        public bool RatingModuleDone { get; set; }
        public bool PublishModuleDone { get; set; }
    }

    public static class CompetitionExtension
    {

        public static CompetitionViewModel ToViewModel(this Competition model)
        {
            return new CompetitionViewModel
            {
                ID = model.ID,
                NameArabic = model.NameArabic,
                NamEnglish = model.NamEnglish,
                Address= model.Address,
                CamelsCount=model.CamelsCount,
                From=model.From,
                To=model.To,
                UserName = model.User?.UserName,
                CategoryArabicName = model.Category?.NameArabic,
                CategoryEnglishName = model.Category?.NameEnglish,
              
            };
        }
    }
}

