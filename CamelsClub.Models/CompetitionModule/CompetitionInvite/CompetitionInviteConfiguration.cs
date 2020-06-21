using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CompetitionInviteConfiguration : EntityTypeConfiguration<CompetitionInvite>
    {
        public CompetitionInviteConfiguration()
        {
            this.ToTable("CompetitionInvite", "Competition");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.HasRequired<Competition>(x => x.Competition)
                .WithMany(x=>x.CompetitionInvites)
                .HasForeignKey<int>(x => x.CompetitionID);

            this.HasRequired<User>(x => x.User)
                .WithMany(x => x.CompetitionInvites)
                .HasForeignKey<int>(x => x.UserID);
            this.HasOptional<CompetitionChecker>(x => x.Checker)
               .WithMany(x => x.Invites)
               .HasForeignKey<int?>(x => x.CheckerID)
               .WillCascadeOnDelete(false);

        }
    }
}
