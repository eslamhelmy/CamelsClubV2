using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CamelsClub.Data.Context
{
    public class CamelsClubContext : DbContext
    {
        public CamelsClubContext() : base("name=CamelsClubDB")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserRoleConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new ActionConfiguration());
            modelBuilder.Configurations.Add(new CommentConfiguration());
            modelBuilder.Configurations.Add(new CommentDocumentConfiguration());
            modelBuilder.Configurations.Add(new PostConfiguration());
            modelBuilder.Configurations.Add(new PostDocumentConfiguration());
            modelBuilder.Configurations.Add(new PostUserActionConfiguration());
            modelBuilder.Configurations.Add(new CommentUserActionConfiguration());
            modelBuilder.Configurations.Add(new UserConfirmationMessageConfiguration());
            modelBuilder.Configurations.Add(new TokenConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new CamelConfiguration());
            modelBuilder.Configurations.Add(new CamelDocumentConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new CamelGroupConfiguration());
            modelBuilder.Configurations.Add(new GenderConfigConfiguration());
            modelBuilder.Configurations.Add(new GenderConfigDetailConfiguration());
            modelBuilder.Configurations.Add(new UserProfileConfiguration());
            modelBuilder.Configurations.Add(new FriendRequestConfiguration());
            modelBuilder.Configurations.Add(new FriendConfiguration());
            modelBuilder.Configurations.Add(new ProfileImageConfiguration());
            modelBuilder.Configurations.Add(new ProfileVideoConfiguration());
            modelBuilder.Configurations.Add(new CompetitionConfiguration()); 
            modelBuilder.Configurations.Add(new CamelCompetitionConfiguration());
            modelBuilder.Configurations.Add(new CompetitionRewardConfiguration());
            modelBuilder.Configurations.Add(new CompetitionInviteConfiguration());
            modelBuilder.Configurations.Add(new CompetitionRefereeConfiguration());
            modelBuilder.Configurations.Add(new NotificationTypeConfiguration());
            modelBuilder.Configurations.Add(new NotificationConfiguration());
            modelBuilder.Configurations.Add(new CompetitionCheckerConfiguration());
            modelBuilder.Configurations.Add(new ReportReasonConfiguration());
            modelBuilder.Configurations.Add(new IssueReportConfiguration());
            modelBuilder.Configurations.Add(new BlockedFriendConfiguration());
            modelBuilder.Configurations.Add(new CheckerApproveConfiguration());
            modelBuilder.Configurations.Add(new ApplicationLogConfiguration());
            modelBuilder.Configurations.Add(new CamelSpecificationConfiguration());
            modelBuilder.Configurations.Add(new RefereeCamelSpecificationReviewConfiguration());
            modelBuilder.Configurations.Add(new MessageConfiguration());
            modelBuilder.Configurations.Add(new ConditionsAndTermsConfiguration());
            modelBuilder.Configurations.Add(new NotificationSettingConfiguration());
            modelBuilder.Configurations.Add(new UserNotificationSettingConfiguration());
            modelBuilder.Configurations.Add(new NotificationSettingValueConfiguration());
            modelBuilder.Configurations.Add(new CompetitionTeamRewardConfiguration());
            modelBuilder.Configurations.Add(new CompetitionSpecificationConfiguration());
            modelBuilder.Configurations.Add(new CompetitionRefereeAllocateConfiguration());
            modelBuilder.Configurations.Add(new CompetitionAllocateConfiguration());
            modelBuilder.Configurations.Add(new ApprovedGroupConfiguration());
            modelBuilder.Configurations.Add(new ReviewApproveConfiguration());
            modelBuilder.Configurations.Add(new PageConfiguration());
            modelBuilder.Configurations.Add(new PermissionConfiguration());
            modelBuilder.Configurations.Add(new PagePermissionConfiguration());

            
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Models.Action> Actions { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<CommentDocument> CommentDocuments { get; set; }
        public virtual DbSet<CommentUserAction> CommentUserActions { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostDocument> PostDocuments { get; set; }
        public virtual DbSet<PostUserAction> PostUserActions { get; set; }
        public virtual DbSet<UserConfirmationMessage> UserConfirmationMessages { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Camel> Camels { get; set; }
        public virtual DbSet<CamelDocument> CamelDocuments { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<CamelGroup> CamelGroups { get; set; }
        public virtual DbSet<GenderConfig> GenderConfigs { get; set; }
        public virtual DbSet<GenderConfigDetail> GenderConfigDetails { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<FriendRequest> FriendRequests { get; set; }
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<ProfileImage> ProfileImages { get; set; }
        public virtual DbSet<ProfileVideo> ProfileVideos { get; set; }
        public virtual DbSet<Competition> Competitions { get; set; }
        public virtual DbSet<CamelCompetition> CamelCompetitions { get; set; }
        public virtual DbSet<CompetitionReward> CompetitionRewards { get; set; }
        public virtual DbSet<CompetitionInvite> CompetitionInvites { get; set; }
        public virtual DbSet<CompetitionReferee> CompetitionReferees { get; set; }
        public virtual DbSet<CompetitionChecker> CompetitionCheckers { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<ReportReason> ReportReasons { get; set; }
        public virtual DbSet<IssueReport> IssueReports { get; set; }
        public virtual DbSet<BlockedFriend> BlockedFriends { get; set; }
        public virtual DbSet<CheckerApprove> CheckerApproves { get; set; }
        public virtual DbSet<ApplicationLog> ApplicationLogs { get; set; }
        public virtual DbSet<CamelSpecification> CamelSpecifications { get; set; }
        public virtual DbSet<RefereeCamelSpecificationReview> RefereeCamelSpecificationReviews { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<ConditionsAndTerms> ConditionsAndTerms { get; set; }
        public virtual DbSet<NotificationSetting> NotificationSettings { get; set; }
        public virtual DbSet<UserNotificationSetting> UserNotificationSettings { get; set; }
        public virtual DbSet<NotificationSettingValue> NotificationSettingValues { get; set; }
        public virtual DbSet<CompetitionSpecification> CompetitionSpecifications { get; set; }
        public virtual DbSet<CompetitionTeamReward> CompetitionTeamRewards { get; set; }
        public virtual DbSet<CompetitionAllocate> CompetitionAllocates { get; set; }
        public virtual DbSet<ApprovedGroup> ApprovedGroups { get; set; }
        public virtual DbSet<CompetitionRefereeAllocate> RefereeAllocates { get; set; }
        public virtual DbSet<ReviewApprove> ReviewApproves { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<PagePermission> PagePermissions { get; set; }



    }
}
