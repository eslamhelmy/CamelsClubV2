using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class ApprovedGroupConfiguration : EntityTypeConfiguration<ApprovedGroup>
    {
        public ApprovedGroupConfiguration()
        {
            this.ToTable("ApprovedGroup", "Competition");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.HasRequired<Competition>(x => x.Competition)
                .WithMany(x=>x.ApprovedGroups)
                .HasForeignKey<int>(x => x.CompetitionID);

            this.HasRequired<Group>(x => x.Group)
                .WithMany()
                .HasForeignKey<int>(x => x.GroupID);
           
        }
    }
}
