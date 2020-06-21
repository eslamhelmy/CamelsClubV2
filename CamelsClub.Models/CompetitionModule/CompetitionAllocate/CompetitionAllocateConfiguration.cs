using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CompetitionAllocateConfiguration : EntityTypeConfiguration<CompetitionAllocate>
    {
        public CompetitionAllocateConfiguration()
        {
            this.ToTable("CompetitionAllocate", "Competition");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.HasRequired<CompetitionChecker>(x => x.CompetitionChecker)
                .WithMany(x=>x.Allocates)
                .HasForeignKey<int>(x => x.CompetitionCheckerID);

            this.HasRequired<Group>(x => x.Group)
                .WithMany(x => x.Allocates)
                .HasForeignKey<int>(x => x.GroupID);
           
        }
    }
}
