using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class PostDocumentConfiguration : EntityTypeConfiguration<PostDocument>
    {
        public PostDocumentConfiguration()
        {
            this.ToTable("PostDocument", "HomeModule");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.FileName).IsRequired();
            //this.Property(x => x.Path).IsRequired();
            this.HasRequired<Post>(x => x.Post)
                .WithMany(x => x.PostDocuments)
                .HasForeignKey<int>(x => x.PostID);
        }
    }
}
