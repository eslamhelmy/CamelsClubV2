using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels.Enums
{
    public enum TypeOfNotification
    {
        UserRequestForJoinCompetition = 1,
        NewPost = 2,
        NewComment=3,
        NewFriendRequest=4,
        ApproveFriendRequest=5,
        CompetitionJion=6,
        LikePost =7,
        LikeComment=8,
        RefereeRequestForJoinCompetition = 9,//لجنة التحكيم
        CheckerRequestForJoinCompetition = 10 ,//لجنة التميز
        RefereeCamelReview=11, //تقييم الجمل  بواسطة احد حكام المسابقة
        ComeleteCamelsReview=12,  //انتهاء تقييم جميع الجمال التابعة لمالك معين
        RejectFriendRequest = 13,
      

    }
}
