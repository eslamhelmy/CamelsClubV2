using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class TokenConfiguration : EntityTypeConfiguration<Token>
    {
        public TokenConfiguration()
        {
            this.ToTable("Token","Identity");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.TokenGUID).IsRequired();
            this.HasRequired<User>(x => x.User)
                         .WithMany(x => x.Tokens)
                         .HasForeignKey<int>(x => x.UserID);

        }
    }
}
