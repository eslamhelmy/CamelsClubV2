using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class NotificationSettingValueConfiguration : EntityTypeConfiguration<NotificationSettingValue>
    {
        public NotificationSettingValueConfiguration()
        {
            this.ToTable("NotificationSettingValue", "Notification");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.TextArabic).IsRequired();
            this.HasRequired<NotificationSetting>(x => x.NotificationSetting)
          .WithMany(x => x.Values)
          .HasForeignKey<int>(x => x.NotificationSettingID).WillCascadeOnDelete(false);

        }
    }
}
