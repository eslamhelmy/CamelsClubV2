using CamelsClub.Models;
using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public interface ITokenRepository : IGenericRepository<Token>
    {
         NotificationReceiverTokensViewModels GetDevicesIDsByUserID(int userID);
        NotificationReceiverTokensViewModels GetDevicesIDsByListOfUserIDs(List<int> usersIDs);
    }
}
