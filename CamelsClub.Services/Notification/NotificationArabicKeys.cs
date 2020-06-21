using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public static class NotificationArabicKeys
    {
        public static string NewPost { get; private set; } = "تمت عملية اضافة منشور من قبل";
        public static string NewComment { get; private set; } = "هناك تعليق على منشور خاص بك";
        public static string NewCompetitionAnnounceToCompetitor { get; private set; } = "تم دعوتك للاشتراك بالمسابقة ";
        public static string NewCompetitionAnnounceToChecker { get; private set; } = "تم دعوتك للاشتراك فى تمييز المسابقة ";
        public static string NewCompetitionAnnounceToReferee { get; private set; } = "تم دعوتك للاشتراك في تحكييم  ";
        public static string PublishCompetition { get; private set; } = "تم نشر نتائج المسابقة";

    }
}
