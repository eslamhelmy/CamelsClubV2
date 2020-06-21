namespace CamelsClub.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init66 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Competition.CompetitionChecker",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CompetitionID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("Competition.Competition", t => t.CompetitionID)
                .ForeignKey("Identity.User", t => t.UserID)
                .Index(t => t.CompetitionID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Competition.CompetitionChecker", "UserID", "Identity.User");
            DropForeignKey("Competition.CompetitionChecker", "CompetitionID", "Competition.Competition");
            DropIndex("Competition.CompetitionChecker", new[] { "UserID" });
            DropIndex("Competition.CompetitionChecker", new[] { "CompetitionID" });
            DropTable("Competition.CompetitionChecker");
        }
    }
}
