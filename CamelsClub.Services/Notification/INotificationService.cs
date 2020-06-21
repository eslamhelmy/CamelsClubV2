using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface INotificationService
    {
        int GetUnseenNotificationCount(int userID);
        UnSeenNotificationViewModel GetUnSeenNotificationsAndMessagesCount(int userID);

        NotificationPagingViewModel<NotificationViewModel> GetMyNotifications(int userID , string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        void SendNotifictionForInvites(NotificationCreateViewModel notifucation, List<CompetitionInviteCreateViewModel> Invites);
        void SendNotifictionForFriends(NotificationCreateViewModel notifucation, List<int> UsersIDs);
       void SendNotifictionForUser(NotificationCreateViewModel notifucation);
        void SendNotifictionForCheckers(NotificationCreateViewModel notifucation, List<CompetitionCheckerCreateViewModel> Checkers);

        void SendNotifictionForReferees(NotificationCreateViewModel notifucation, List<CompetitionRefereeCreateViewModel> Referees);
        bool IsExist(int Id);
        void UpdateNotificationStatus(int Id);
        List<NotificationTypeViewModel> GetTypes();
    }
}
