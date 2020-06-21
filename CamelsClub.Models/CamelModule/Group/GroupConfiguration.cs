using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class GroupConfiguration : EntityTypeConfiguration<Group>
    {
        public GroupConfiguration()
        {
            this.ToTable("Group", "Camel");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.NameArabic).IsRequired();
            this.Property(x => x.NameEnglish).IsRequired();
            this.HasRequired<User>(x => x.User)
           .WithMany(x => x.Groups)
           .HasForeignKey<int>(x => x.UserID).WillCascadeOnDelete(false);
            this.HasRequired<Category>(x => x.Category)
          .WithMany(x => x.Groups)
          .HasForeignKey<int>(x => x.CategoryID).WillCascadeOnDelete(false);

        }
    }
}
