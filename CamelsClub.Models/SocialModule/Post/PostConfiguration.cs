using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class PostConfiguration : EntityTypeConfiguration<Post>
    {
        public PostConfiguration()
        {
            this.ToTable("Post", "HomeModule");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.Text).IsOptional();
            this.HasRequired<User>(x => x.User)
                .WithMany(x => x.Posts)
                .HasForeignKey<int>(x => x.UserID).WillCascadeOnDelete(false);
        }
    }
}
