using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class GenderConfigDetailConfiguration : EntityTypeConfiguration<GenderConfigDetail>
    {
        public GenderConfigDetailConfiguration()
        {
            this.ToTable("GenderConfigDetail", "Camel");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.NameArabic).IsRequired();
            this.HasRequired<GenderConfig>(x => x.GenderConfig)
                .WithMany(x => x.GenderConfigDetails)
                .HasForeignKey<int>(x => x.GenderConfigID);
        }
    }
}
