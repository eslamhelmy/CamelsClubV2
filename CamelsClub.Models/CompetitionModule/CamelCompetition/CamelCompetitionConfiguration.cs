using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CamelCompetitionConfiguration : EntityTypeConfiguration<CamelCompetition>
    {
        public CamelCompetitionConfiguration()
        {
            this.ToTable("CamelCompetition", "Camel");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.HasRequired<Camel>(x => x.Camel)
                .WithMany(x=>x.CamelCompetitions)
                .HasForeignKey<int>(x => x.CamelID);
            this.HasRequired<Competition>(x => x.Competition)
                .WithMany(x=>x.CamelCompetitions)
                .HasForeignKey<int>(x => x.CompetitionID);
            this.HasRequired<CompetitionInvite>(x => x.CompetitionInvite)
               .WithMany(x => x.CamelCompetitions)
               .HasForeignKey<int>(x => x.CompetitionInviteID)
               .WillCascadeOnDelete(false);
            this.HasRequired<Group>(x => x.Group)
            .WithMany(x=> x.CamelCompetitions)
            .HasForeignKey<int>(x => x.GroupID);
           
        }
    }
}
