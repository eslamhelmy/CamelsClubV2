using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public static class NotificationEnglishKeys
    {
        public static string NewPost { get; private set; } = "new post has been added by ";
        public static string NewComment { get; private set; } = "new comment has been added on this post";
        public static string NewCompetitionAnnounceToCompetitor { get; private set; } = "you have been invited as competitor to join this competition ";
        public static string NewCompetitionAnnounceToChecker { get; private set; } = "you have been invited as checker to join this competition ";
        public static string NewCompetitionAnnounceToReferee { get; private set; } = "you have been invited as referee to join this competition ";
        public static string PublishCompetition { get; private set; } = "results is published for competition ";


    }
}
