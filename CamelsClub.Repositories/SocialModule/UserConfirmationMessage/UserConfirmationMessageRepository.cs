using CamelsClub.Data.Context;
using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class UserConfirmationMessageRepository : GenericRepository<UserConfirmationMessage> , IUserConfirmationMessageRepository
    {
        public UserConfirmationMessageRepository(CamelsClubContext context): base(context)
        {

        }
        public string GetNotUsedCode(string phone)
        {
           return
                GetAll().Where(x => x.User.Phone == phone)
                       .Where(x => !x.IsUsed)
                       .Where(x => !x.IsDeleted)
                       .Select(x => x.Code).FirstOrDefault();

        }

        public UserConfirmationMessage GetUserConfirmMessage(string code, int userID)
        {
              return 
                GetAll().
                      FirstOrDefault(ConMsg =>
                                       ConMsg.Code == code &&
                                       ConMsg.UserID == userID &&
                                        !ConMsg.IsUsed &&
                                        !ConMsg.IsDeleted);
        }

        public bool IsValidCode(string verificationCode, int userID)
        {
            return
                 GetAll()
                    .Any(x => x.Code == verificationCode && x.UserID == userID && !x.IsDeleted && !x.IsUsed );     
        }
     

    }
}
