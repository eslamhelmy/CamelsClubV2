using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class IssueReportConfiguration : EntityTypeConfiguration<IssueReport>
    {
        public IssueReportConfiguration()
        {
            this.ToTable("IssueReport", "Report");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.Text).IsOptional();
            this.HasRequired<User>(x => x.User)
                .WithMany(x => x.IssueReports)
                .HasForeignKey<int>(x => x.UserID).WillCascadeOnDelete(false);
            this.HasRequired<Post>(x => x.Post)
           .WithMany(x => x.IssueReports)
           .HasForeignKey<int>(x => x.PostID).WillCascadeOnDelete(false);

           this.HasRequired<ReportReason>(x => x.ReportReason)
          .WithMany(x=>x.IssueReports)
          .HasForeignKey<int>(x => x.ReportReasonID).WillCascadeOnDelete(false);

        }
    }
}
