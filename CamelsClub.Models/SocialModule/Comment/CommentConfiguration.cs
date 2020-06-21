using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CommentConfiguration : EntityTypeConfiguration<Comment>
    {
        public CommentConfiguration()
        {
            this.ToTable("Comment","HomeModule");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.Text).IsRequired();
            this.HasRequired<User>(x => x.User)
                        .WithMany()
                        .HasForeignKey<int>(x => x.UserID)
                        .WillCascadeOnDelete(false);
            this.HasRequired<Post>(x => x.Post)
                        .WithMany(x => x.Comments)
                        .HasForeignKey<int>(x => x.PostID);
            ////recurisve
            //  this.HasOptional<Comment>(x => x.ParentComment)
            //    .WithMany().HasForeignKey<int>(x => x.ParentCommentID);
            this.Property(x => x.ParentCommentID).IsOptional();
        }
    }
}
