using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class ConditionsAndTermsConfiguration : EntityTypeConfiguration<ConditionsAndTerms>
    {
        public ConditionsAndTermsConfiguration()
        {
            this.ToTable("ConditionsAndTerms", "Social");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.TextArabic).IsRequired();
            this.Property(x => x.TextEnglish).IsRequired();
        }
    }
}
