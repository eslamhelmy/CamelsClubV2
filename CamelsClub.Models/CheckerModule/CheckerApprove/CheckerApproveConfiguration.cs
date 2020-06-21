using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CheckerApproveConfiguration : EntityTypeConfiguration<CheckerApprove>
    {
        public CheckerApproveConfiguration()
        {
            this.ToTable("CheckerApprove", "Checker");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            
            //this.HasRequired<User>(x => x.User)
            //    .WithMany(x => x.InvitesIApproved)
            //    .HasForeignKey<int>(x => x.UserID).WillCascadeOnDelete(false);
            this.HasRequired<CompetitionChecker>(x => x.CompetitionChecker)
                .WithMany(x => x.CamelsIApproved)
                .HasForeignKey<int>(x => x.CompetitionCheckerID).WillCascadeOnDelete(false);

            this.HasRequired<CamelCompetition>(x => x.CamelCompetition)
               .WithMany(x=>x.CheckerApprovers)
               .HasForeignKey<int>(x => x.CamelCompetitionID).WillCascadeOnDelete(false);


        }
    }
}
