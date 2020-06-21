using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class NotificationTypeConfiguration : EntityTypeConfiguration<NotificationType>
    {
        public NotificationTypeConfiguration()
        {
            this.ToTable("NotificationType", "Notification");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.NameArabic).IsRequired();
            this.Property(x => x.NameEnglish).IsRequired();
        }
    }
}
