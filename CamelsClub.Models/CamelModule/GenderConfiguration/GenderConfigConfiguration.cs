using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class GenderConfigConfiguration : EntityTypeConfiguration<GenderConfig>
    {
        public GenderConfigConfiguration()
        {
            this.ToTable("GenderConfig","Camel");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.FromAge).IsRequired();
            this.Property(x => x.ToAge).IsRequired();
        }
    }
}
