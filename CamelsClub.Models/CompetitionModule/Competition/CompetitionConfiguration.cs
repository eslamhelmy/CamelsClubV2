using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class CompetitionConfiguration : EntityTypeConfiguration<Competition>
    {
        public CompetitionConfiguration()
        {
            this.ToTable("Competition", "Competition");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.NameArabic).IsRequired();
            this.Property(x => x.CamelsCount).IsRequired();
            this.Property(x => x.From).IsRequired();
            this.Property(x => x.To).IsRequired();
            this.Property(x => x.NamEnglish).IsOptional();
            this.Property(x => x.Completed).IsOptional();
            this.HasRequired<User>(x => x.User)
                .WithMany(x => x.Competitions)
                .HasForeignKey<int>(x => x.UserID).WillCascadeOnDelete(false);

            this.HasRequired<Category>(x => x.Category)
         .WithMany(x => x.Competitions)
         .HasForeignKey<int>(x => x.CategoryID).WillCascadeOnDelete(false);

        }
    }
}
