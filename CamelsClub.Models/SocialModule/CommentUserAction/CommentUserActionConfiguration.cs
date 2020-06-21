using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CommentUserActionConfiguration : EntityTypeConfiguration<CommentUserAction>
    {
        public CommentUserActionConfiguration()
        {
            this.ToTable("CommentUserAction", "HomeModule");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.HasRequired<Comment>(x => x.Comment)
                .WithMany(x => x.CommentUserActions)
                .HasForeignKey<int>(x => x.CommentID);
            this.HasRequired<User>(x => x.User)
                .WithMany(x => x.CommentUserActions)
                .HasForeignKey<int>(x => x.UserID).WillCascadeOnDelete(false);
            this.HasRequired<Action>(x => x.Action)
                 .WithMany(x => x.CommentUserActions)
                .HasForeignKey<int>(x => x.ActionID);
        }
    }
}
