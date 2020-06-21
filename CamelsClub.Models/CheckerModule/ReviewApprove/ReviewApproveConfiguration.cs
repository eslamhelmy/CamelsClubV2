using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class ReviewApproveConfiguration : EntityTypeConfiguration<ReviewApprove>
    {
        public ReviewApproveConfiguration()
        {
            this.ToTable("ReviewApprove", "Checker");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();

            this.Property(x => x.Status).IsRequired();

            this.HasRequired<CompetitionChecker>(x => x.CompetitionChecker)
                .WithMany()
                .HasForeignKey<int>(x => x.CheckerID).WillCascadeOnDelete(false);

            this.HasRequired<CheckerApprove>(x => x.CheckerApprove)
               .WithMany(x=>x.Reviews)
               .HasForeignKey<int>(x => x.CheckerApproveID).WillCascadeOnDelete(false);


        }
    }
}
