namespace CamelsClub.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            AddColumn("Notification.NotificationSettingValue", "IsDefault", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Notification.NotificationSettingValue", "IsDefault");
        }
    }
}
