using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class NotificationCreateViewModel
    {
        public int ID { get; set; }
        public string ContentArabic { get; set; }
        public string ContentEnglish { get; set; }
        public int NotificationTypeID { get; set; }
        public string EngNotificationType { get; set; }
        public string ArbNotificationType { get; set; }
        public int SourceID { get; set; }
        public string SourceName { get; set; }
        public string DestinationName { get; set; }
        public int DestinationID { get; set; }
        public int? PostID { get; set; }
        public int? CommentID { get; set; }
        public int? MessageID { get; set; }
        public int? FriendRequestID { get; set; }
        public int? ActionID { get; set; }
        public int? CompetitionID { get; set; }
        public string UserImagePath { get; set; }
        public string CompetitionImagePath { get; set; }

        public ViewModels.Enums.NotificationType Type { get; set; }
    }

    public static partial class NotificationExtensions
    {
        public static Notification ToModel(this NotificationCreateViewModel viewModel)
        {
            return new Notification
            {
                ID = viewModel.ID,
                ContentEnglish = viewModel.ContentEnglish,
                ContentArabic = viewModel.ContentArabic,
                NotificationTypeID = viewModel.NotificationTypeID,
                SourceID = viewModel.SourceID,
                DestinationID = viewModel.DestinationID,
                PostID = viewModel.PostID,
                CommentID = viewModel.CommentID,
                MessageID = viewModel.MessageID,
                ActionID = viewModel.ActionID,
                FriendRequestID = viewModel.FriendRequestID,
                CompetitionID = viewModel.CompetitionID
                

            };
        }


    }
}
