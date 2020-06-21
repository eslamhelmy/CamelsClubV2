using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CompetitionCheckerConfiguration : EntityTypeConfiguration<CompetitionChecker>
    {
        public CompetitionCheckerConfiguration()
        {
            this.ToTable("CompetitionChecker", "Competition");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.HasRequired<Competition>(x => x.Competition)
                .WithMany(x=>x.CompetitionCheckers)
                .HasForeignKey<int>(x => x.CompetitionID)
                .WillCascadeOnDelete(false);

            this.HasRequired<User>(x => x.User)
                .WithMany(x => x.CompetitionCheckers)
                .HasForeignKey<int>(x => x.UserID)
                .WillCascadeOnDelete(false);
        }
    }
}
