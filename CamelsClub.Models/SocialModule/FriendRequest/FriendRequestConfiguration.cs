using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class FriendRequestConfiguration : EntityTypeConfiguration<FriendRequest>
    {
        public FriendRequestConfiguration()
        {
            this.ToTable("FriendRequest", "HomeModule");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.Status).IsRequired();

            this.HasRequired<User>(x => x.FromUser)
                .WithMany(x => x.FromFriendRequests)
                .HasForeignKey<int>(x => x.FromUserID).WillCascadeOnDelete(false);
            this.HasRequired<User>(x => x.ToUser)
               .WithMany(x => x.ToFriendRequests)
               .HasForeignKey<int>(x => x.ToUserID).WillCascadeOnDelete(false);


        }
    }
}
