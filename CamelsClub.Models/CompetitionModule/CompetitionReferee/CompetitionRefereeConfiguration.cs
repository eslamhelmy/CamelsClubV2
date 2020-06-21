using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CompetitionRefereeConfiguration : EntityTypeConfiguration<CompetitionReferee>
    {
        public CompetitionRefereeConfiguration()
        {
            this.ToTable("CompetitionReferee", "Competition");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.HasRequired<Competition>(x => x.Competition)
                .WithMany(x => x.CompetitionReferees)
                .HasForeignKey<int>(x => x.CompetitionID);

            this.HasRequired<User>(x => x.User)
                .WithMany(x => x.CompetitionReferees)
                .HasForeignKey<int>(x => x.UserID);
        }
    }
}
