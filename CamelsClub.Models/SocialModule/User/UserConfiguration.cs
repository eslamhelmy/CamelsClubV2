using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            this.ToTable("User","Identity");
            this.HasKey<int>(x => x.ID);
           // this.HasIndex(u => u.Phone).IsUnique();

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.Email).IsOptional();
            this.Property(x => x.Phone).IsRequired();
            this.Property(x => x.NID).IsRequired();

            //one to one relationship
            this.HasOptional<UserProfile>(x => x.UserProfile)
                .WithRequired(x => x.User);
        }
    }
}
