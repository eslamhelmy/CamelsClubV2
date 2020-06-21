namespace CamelsClub.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("HomeModule.Friend", "IsBlocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("HomeModule.Friend", "IsBlocked");
        }
    }
}
