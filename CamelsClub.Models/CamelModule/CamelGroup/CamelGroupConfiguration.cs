using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CamelGroupConfiguration : EntityTypeConfiguration<CamelGroup>
    {
        public CamelGroupConfiguration()
        {
            this.ToTable("CamelGroup", "Camel");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.HasRequired<Camel>(x => x.Camel)
                .WithMany()
                .HasForeignKey<int>(x => x.CamelID);
            this.HasRequired<Group>(x => x.Group)
                .WithMany(x=> x.CamelGroups)
                .HasForeignKey<int>(x => x.GroupID);
        }
    }
}
