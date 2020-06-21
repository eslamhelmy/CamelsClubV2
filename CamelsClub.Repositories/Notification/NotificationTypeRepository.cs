using CamelsClub.Data.Context;
using CamelsClub.Models;

namespace CamelsClub.Repositories
{
    public class NotificationTypeRepository : GenericRepository<NotificationType>, INotificationTypeRepository
    {
        public NotificationTypeRepository(CamelsClubContext context) : base(context)
        {

        }
    }
}
