using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        void MakeNotificationIsSeen(int notificationId);
    }
}
