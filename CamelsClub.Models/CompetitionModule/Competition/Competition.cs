using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class Competition : BaseModel
    {
        public string NameArabic { get; set; }
        public string NamEnglish { get; set; }
        public string Address { get; set; }
        public string ConditionText { get; set; }
        public int CamelsCount { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime CompetitorsEndJoinDate { get; set; }
        public int CompetitionType { get; set; }
        public string Image { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public int MinimumCompetitorsCount { get; set; }
        public int MaximumCompetitorsCount { get; set; }
        public int MinimumRefereesCount { get; set; }
        public int MaximumRefereesCount { get; set; }
        public int MinimumCheckersCount { get; set; }
        public int MaximumCheckersCount { get; set; }
        public bool ShowReferees { get; set; }
        public bool ShowCheckers { get; set; }
        public bool ShowCompetitors { get; set; }
        public decimal RefereesVotePercentage { get; set; }
        public decimal PeopleVotePercentage { get; set; }
        public DateTime? Completed { get; set; }
        public DateTime? Published { get; set; }
        public DateTime? StartedDate { get; set; }
        public DateTime? CheckersAllocatedDate { get; set; }
        public DateTime? RefereesAllocatedDate { get; set; }
        public DateTime? CheckerPickupTeamDateTime { get; set; }
        public DateTime? RefereePickupTeamDateTime { get; set; }
        
        public virtual ICollection<CamelCompetition> CamelCompetitions { get; set; }
        public virtual ICollection<CompetitionReward> CompetitionRewards { get; set; }
        public virtual ICollection<CompetitionInvite> CompetitionInvites { get; set; }
        public virtual ICollection<CompetitionReferee> CompetitionReferees { get; set; }
        public virtual ICollection<CompetitionCondition> CompetitionConditions { get; set; }
        public virtual ICollection<CompetitionChecker> CompetitionCheckers { get; set; }
        public virtual ICollection<CompetitionSpecification> CompetitionSpecifications { get; set; }
        public virtual ICollection<CompetitionTeamReward> CompetitionTeamRewards { get; set; }
        public virtual ICollection<ApprovedGroup> ApprovedGroups { get; set; }
    }
}
