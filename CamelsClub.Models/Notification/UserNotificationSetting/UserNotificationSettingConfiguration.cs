using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class UserNotificationSettingConfiguration : EntityTypeConfiguration<UserNotificationSetting>
    {
        public UserNotificationSettingConfiguration()
        {
            this.ToTable("UserNotificationSetting", "User");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            
            this.HasRequired<NotificationSetting>(x => x.NotificationSetting)
           .WithMany(x => x.UserNotificationSettings)
           .HasForeignKey<int>(x => x.NotificationSettingID);

            this.HasRequired<User>(x => x.User)
              .WithMany(x => x.UserNotificationSettings)
              .HasForeignKey<int?>(x => x.UserID).WillCascadeOnDelete(false);
           
            this.HasRequired<NotificationSettingValue>(x => x.NotificationSettingValue)
              .WithMany(x=>x.Users)
              .HasForeignKey<int>(x => x.NotificationSettingValueID);

        }
    }
}
