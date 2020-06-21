namespace CamelsClub.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Friend.BlockedFriend",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        BlockedFriendID = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("Identity.User", t => t.BlockedFriendID)
                .ForeignKey("Identity.User", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.BlockedFriendID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Friend.BlockedFriend", "UserID", "Identity.User");
            DropForeignKey("Friend.BlockedFriend", "BlockedFriendID", "Identity.User");
            DropIndex("Friend.BlockedFriend", new[] { "BlockedFriendID" });
            DropIndex("Friend.BlockedFriend", new[] { "UserID" });
            DropTable("Friend.BlockedFriend");
        }
    }
}
