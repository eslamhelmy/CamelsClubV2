using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CompetitionRewardConfiguration : EntityTypeConfiguration<CompetitionReward>
    {
        public CompetitionRewardConfiguration()
        {
            this.ToTable("CompetitionReward", "Competition");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.HasRequired<Competition>(x => x.Competition)
                .WithMany(x=>x.CompetitionRewards)
                .HasForeignKey<int>(x => x.CompetitionID);
            this.HasOptional<User>(x => x.Sponsor)
              .WithMany()
              .HasForeignKey<int?>(x => x.SponsorID);

        }
    }
}
