using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public interface IUserConfirmationMessageRepository : IGenericRepository<UserConfirmationMessage>
    {
      
        bool IsValidCode(string v, int userID);
        UserConfirmationMessage GetUserConfirmMessage(string code, int userID);
        string GetNotUsedCode(string phone);
    }
}
