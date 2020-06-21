using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class PostUserActionConfiguration : EntityTypeConfiguration<PostUserAction>
    {
        public PostUserActionConfiguration()
        {
            this.ToTable("PostUserAction", "HomeModule");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.HasRequired<Post>(x => x.Post)
                .WithMany(x => x.PostUserActions)
                .HasForeignKey<int>(x => x.PostID);
            this.HasRequired<User>(x => x.User)
                .WithMany(x => x.PostUserActions)
                .HasForeignKey<int>(x => x.UserID).WillCascadeOnDelete(false);
            this.HasRequired<Action>(x => x.Action)
                 .WithMany(x => x.PostUserActions)
                .HasForeignKey<int>(x => x.ActionID);
        }
    }
}
