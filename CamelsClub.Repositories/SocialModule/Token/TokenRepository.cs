using CamelsClub.Data.Context;
using CamelsClub.Models;
using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class TokenRepository : GenericRepository<Token> , ITokenRepository
    {
        public TokenRepository(CamelsClubContext context): base(context)
        {
            ///test
            /////testtt 
            //again
        }



        public NotificationReceiverTokensViewModels GetDevicesIDsByUserID(int userID)
        {
            var query = this.GetAll().Where(c => c.UserID == userID).Select(c => new NotificationReceiverTokensViewModels
            {
                DevicesIDs = c.User.Tokens.Where(t =>t.DeviceID != "" && t.DeviceID != null && t.ExpireDate > DateTime.Now && t.LoggedOutDate == null).OrderBy(t => t.ID).Select(t => t.DeviceID).Distinct(),
                //ConnectionIDs = c.User.Tokens.Where(t => t.SignalRConnectionID != null && t.ExpirationDate > DateTime.Now && t.LoggedOutDate == null).Select(t => t.SignalRConnectionID)
            });

            return query.FirstOrDefault();
        }
        public NotificationReceiverTokensViewModels GetDevicesIDsByListOfUserIDs(List<int> usersIDs)
        {
            var query = this.GetAll().Where(c => usersIDs.Contains(c.UserID) && !c.IsDeleted).Select(c => new NotificationReceiverTokensViewModels
            {
                DevicesIDs = c.User.Tokens.Where(t => t.DeviceID != null && t.ExpireDate > DateTime.Now && t.LoggedOutDate == null).OrderBy(t => t.ID).Select(t => t.DeviceID).Distinct(),
                //ConnectionIDs = c.User.Tokens.Where(t => t.SignalRConnectionID != null && t.ExpirationDate > DateTime.Now && t.LoggedOutDate == null).Select(t => t.SignalRConnectionID)
            });

            return query.FirstOrDefault();
        }
    }
}
