using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CompetitionConditionConfiguration : EntityTypeConfiguration<CompetitionCondition>
    {
        public CompetitionConditionConfiguration()
        {
            this.ToTable("CompetitionCondition", "Competition");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.HasRequired<Competition>(x => x.Competition)
                .WithMany(x=>x.CompetitionConditions)
                .HasForeignKey<int>(x => x.CompetitionID);
        }
    }
}
