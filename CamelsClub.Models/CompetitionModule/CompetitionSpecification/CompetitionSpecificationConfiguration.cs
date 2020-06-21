using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CompetitionSpecificationConfiguration : EntityTypeConfiguration<CompetitionSpecification>
    {
        public CompetitionSpecificationConfiguration()
        {
            this.ToTable("CompetitionSpecification", "Competition");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.MaxAllowedValue).IsRequired();
            this.HasRequired<CamelSpecification>(x => x.CamelSpecification)
                .WithMany(x=>x.CompetitionSpecifications)
                .HasForeignKey<int>(x => x.CamelSpecificationID);
            this.HasRequired<Competition>(x => x.Competition)
                .WithMany(x=>x.CompetitionSpecifications)
                .HasForeignKey<int>(x => x.CompetitionID);
        }
    }
}
