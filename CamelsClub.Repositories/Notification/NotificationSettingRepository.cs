using CamelsClub.Data.Context;
using CamelsClub.Models;

namespace CamelsClub.Repositories
{
    public class NotificationSettingRepository : GenericRepository<NotificationSetting>, INotificationSettingRepository
    {
        public NotificationSettingRepository(CamelsClubContext context) : base(context)
        {

        }
    }
}
