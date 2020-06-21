using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class ApplicationLogConfiguration : EntityTypeConfiguration<ApplicationLog>
    {
        public ApplicationLogConfiguration()
        {
            this.ToTable("ApplicationLog", "Log");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.IP).IsRequired();
            this.Property(x => x.URL).IsRequired();

        }
    }
}
