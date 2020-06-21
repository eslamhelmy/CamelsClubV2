using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class BlockedFriendConfiguration : EntityTypeConfiguration<BlockedFriend>
    {
        public BlockedFriendConfiguration()
        {
            this.ToTable("BlockedFriend", "Friend");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            
            this.HasRequired<User>(x => x.User)
                .WithMany(x => x.BlockedFriends)
                .HasForeignKey<int>(x => x.UserID).WillCascadeOnDelete(false);

            this.HasRequired<User>(x => x.BlockFriend)
               .WithMany(x=>x.FriendsBlockedMe)
               .HasForeignKey<int>(x => x.BlockedFriendID).WillCascadeOnDelete(false);


        }
    }
}
