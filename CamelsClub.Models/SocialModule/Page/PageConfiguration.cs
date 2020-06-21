using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class PageConfiguration : EntityTypeConfiguration<Page>
    {
        public PageConfiguration()
        {
            this.ToTable("Page","Authorization");
            this.HasIndex(x => x.ID).IsUnique();

         //   this.Property(x => x.ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.NameArabic).IsRequired();
            this.Property(x => x.NameEnglish).IsRequired();
     
        }
    }
}
