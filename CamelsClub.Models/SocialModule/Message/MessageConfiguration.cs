using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class MessageConfiguration : EntityTypeConfiguration<Message>
    {
        public MessageConfiguration()
        {
            this.ToTable("Message", "Social");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.Text).IsRequired();
            
            this.HasRequired<User>(x => x.FromUser)
                .WithMany(x => x.SentMessages)
                .HasForeignKey<int>(x => x.FromUserID).WillCascadeOnDelete(false);

            this.HasRequired<User>(x => x.ToUser)
               .WithMany(x => x.ReceivedMessages)
               .HasForeignKey<int>(x => x.ToUserID).WillCascadeOnDelete(false);

        }
    }
}
