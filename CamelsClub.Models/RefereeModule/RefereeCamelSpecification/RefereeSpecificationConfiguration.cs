using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class RefereeCamelSpecificationReviewConfiguration : EntityTypeConfiguration<RefereeCamelSpecificationReview>
    {
        public RefereeCamelSpecificationReviewConfiguration()
        {

            this.ToTable("RefereeCamelSpecificationReview", "Referee");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ActualPercentageValue).IsRequired();
            this.HasRequired<CompetitionReferee>(x => x.CompetitionReferee)
                .WithMany(x => x.MyReviews)
                .HasForeignKey<int>(x => x.CompetitionRefereeID).WillCascadeOnDelete(false);
            this.HasRequired<CamelCompetition>(x => x.CamelCompetition)
               .WithMany(x=>x.RefereeReviews)
               .HasForeignKey<int>(x => x.CamelCompetitionID).WillCascadeOnDelete(false);
            this.HasRequired<CamelSpecification>(x => x.CamelSpecification)
                .WithMany(x => x.RefereeReviews)
                .HasForeignKey<int>(x => x.CamelSpecificationID).WillCascadeOnDelete(false);
        }
    }
}
