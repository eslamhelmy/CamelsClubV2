using CamelsClub.Models;
using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface IUserConfirmationMessageService
    {
        void InsertUserConfirmationMessage(int userID,string code);
        ConfirmationMessageViewModel UpdateUserConfirmationMessage(string code, int userID);
        string ResendCode(string phone);
    }
}
