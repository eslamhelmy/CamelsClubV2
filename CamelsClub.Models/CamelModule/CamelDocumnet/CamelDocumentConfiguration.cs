using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CamelDocumentConfiguration : EntityTypeConfiguration<CamelDocument>
    {
        public CamelDocumentConfiguration()
        {
            this.ToTable("CamelDocument", "Camel");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.FileName).IsRequired();
            //this.Property(x => x.Path).IsRequired();
            this.HasRequired<Camel>(x => x.Camel)
                .WithMany(x => x.CamelDocuments)
                .HasForeignKey<int>(x => x.CamelID);
        }
    }
}
