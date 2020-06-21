using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class UserConfirmationMessageConfiguration : EntityTypeConfiguration<UserConfirmationMessage>
    {
        public UserConfirmationMessageConfiguration()
        {
            this.ToTable("UserConfirmationMessage", "Identity");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.Code).IsRequired();
            this.HasRequired<User>(x => x.User)
                .WithMany(x => x.UserConfirmationMessages)
                .HasForeignKey<int>(x => x.UserID);
        }
    }
}
