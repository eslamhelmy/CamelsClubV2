using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CommentDocumentConfiguration : EntityTypeConfiguration<CommentDocument>
    {
        public CommentDocumentConfiguration()
        {
            this.ToTable("CommentDocument", "HomeModule");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.FileName).IsRequired();
            //this.Property(x => x.Path).IsRequired();
            this.HasRequired<Comment>(x => x.Comment)
                .WithMany(x => x.CommentDocuments)
                .HasForeignKey<int>(x => x.CommentID);
        }
    }
}
