using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class UserRoleConfiguration : EntityTypeConfiguration<UserRole>
    {
        public UserRoleConfiguration()
        {
            this.ToTable("UserRole", "Identity");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.HasRequired<Role>(x => x.Role)
                .WithMany(x => x.UserRoles)
                .HasForeignKey<int>(x => x.RoleID);
            this.HasRequired<User>(x => x.User)
                .WithMany(x => x.UserRoles)
                .HasForeignKey<int>(x => x.UserID);
        }
    }
}
