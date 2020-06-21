using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CompetitionRefereeAllocateConfiguration : EntityTypeConfiguration<CompetitionRefereeAllocate>
    {
        public CompetitionRefereeAllocateConfiguration()
        {
            this.ToTable("CompetitionRefereeAllocate", "Competition");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.HasRequired<CompetitionReferee>(x => x.CompetitionReferee)
                .WithMany(x=>x.Allocates)
                .HasForeignKey<int>(x => x.CompetitionRefereeID);

            this.HasRequired<Group>(x => x.Group)
                .WithMany(x => x.RefereeAllocates)
                .HasForeignKey<int>(x => x.GroupID);
           
        }
    }
}
