using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CamelConfiguration : EntityTypeConfiguration<Camel>
    {
        public CamelConfiguration()
        {
            this.ToTable("Camel", "Camel");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.Name).IsRequired();
            this.Property(x => x.BirthDate).IsRequired();
            this.HasRequired<Category>(x => x.Category)
                .WithMany(x => x.Camels)
                .HasForeignKey<int>(x => x.CategoryID);
            this.HasRequired<User>(x => x.User)
              .WithMany(x => x.Camels)
              .HasForeignKey<int>(x => x.UserID);

            this.HasRequired<GenderConfigDetail>(x => x.GenderConfigDetail)
              .WithMany(x => x.Camels)
              .HasForeignKey<int>(x => x.GenderConfigDetailID);

        }
    }
}
