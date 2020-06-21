using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class PagePermissionConfiguration : EntityTypeConfiguration<PagePermission>
    {
        public PagePermissionConfiguration()
        {
            this.ToTable("PagePermission", "Authorization");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            
            this.HasRequired<Page>(x => x.Page)
                .WithMany(x => x.PagePermissions)
                .HasForeignKey<int>(x => x.PageID);

            this.HasRequired<Permission>(x => x.Permission)
               .WithMany(x => x.PagePermissions)
               .HasForeignKey<int>(x => x.PermissionID);

        }
    }
}
