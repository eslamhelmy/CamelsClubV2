using CamelsClub.Data.Context;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(CamelsClubContext context) : base(context)
        {

        }

        public void  MakeNotificationIsSeen(int notificationId)
        {
            var notification = this.GetAll().Where(not => not.ID == notificationId)
                                            .Where(not=>!not.IsDeleted).FirstOrDefault();

            notification.SeenDateTime = DateTime.Now;
            this.SaveIncluded(notification, "SeenDateTime");
        }





       
    }
}
