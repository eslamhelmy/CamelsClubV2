using CamelsClub.Data.Context;
using CamelsClub.Models;

namespace CamelsClub.Repositories
{
    public class UserNotificationSettingRepository : GenericRepository<UserNotificationSetting>, IUserNotificationSettingRepository
    {
        public UserNotificationSettingRepository(CamelsClubContext context) : base(context)
        {

        }
    }
}
