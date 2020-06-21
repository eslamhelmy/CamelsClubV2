using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class FriendConfiguration : EntityTypeConfiguration<Friend>
    {
        public FriendConfiguration()
        {
            this.ToTable("Friend", "HomeModule");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            
            this.HasRequired<User>(x => x.User)
                .WithMany(x => x.Friends)
                .HasForeignKey<int>(x => x.UserID).WillCascadeOnDelete(false);
            this.HasRequired<User>(x => x.FriendUser)
               .WithMany(x => x.FriendUsers)
               .HasForeignKey<int>(x => x.FriendUserID).WillCascadeOnDelete(false);


        }
    }
}
