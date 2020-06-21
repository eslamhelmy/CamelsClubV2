using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class NotificationConfiguration : EntityTypeConfiguration<Notification>
    {
        public NotificationConfiguration()
        {
            this.ToTable("Notification", "Notification");
            this.HasKey<int>(x => x.ID);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ContentArabic).IsRequired();
            this.Property(x => x.ContentEnglish).IsRequired();

            this.HasRequired<NotificationType>(x => x.NotificationType)
           .WithMany()
           .HasForeignKey<int>(x => x.NotificationTypeID);

            this.HasRequired<User>(x => x.Source)
              .WithMany()
              .HasForeignKey<int?>(x => x.SourceID).WillCascadeOnDelete(false);
            this.HasRequired<User>(x => x.Destination)
           .WithMany()
           .HasForeignKey<int?>(x => x.DestinationID).WillCascadeOnDelete(false); ;

            this.HasOptional<Post>(x => x.Post)
            .WithMany()
            .HasForeignKey<int?>(x => x.PostID);

            this.HasOptional<Comment>(x => x.Comment)
                .WithMany()
                .HasForeignKey<int?>(x => x.CommentID);
            this.HasOptional<FriendRequest>(x => x.FriendRequest)
                .WithMany()
                .HasForeignKey<int?>(x => x.FriendRequestID);

            this.HasOptional<Action>(x => x.Action)
                .WithMany()
                .HasForeignKey<int?>(x => x.ActionID);
            this.HasOptional<Competition>(x => x.Competition)
              .WithMany()
              .HasForeignKey<int?>(x => x.CompetitionID);

            //this.Property(x => x.PostID).IsOptional();
            //this.Property(x => x.CommentID).IsOptional();
            //this.Property(x => x.FriendRequestID).IsOptional();
            //this.Property(x => x.ActionID).IsOptional();

        }
    }
}
