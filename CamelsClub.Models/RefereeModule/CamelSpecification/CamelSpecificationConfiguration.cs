using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CamelSpecificationConfiguration : EntityTypeConfiguration<CamelSpecification>
    {
        public CamelSpecificationConfiguration()
        {

            this.ToTable("CamelSpecification", "Referee");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.SpecificationEnglish).IsRequired();
            this.Property(x => x.SpecificationArabic).IsRequired();


        }
    }
}
