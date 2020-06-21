using CamelsClub.ViewModels;
using System.Collections.Generic;

namespace CamelsClub.Services
{
    public interface ITokenService  
    {
        //NotificationReceiverTokensViewModels GetConnectionIDsAndDevicesIDs(int userID);
        TokenViewModel AddToken(string token, string userAgent, string ip);
        TokenViewModel AddTokenForMobile(string token, string deviceID, string userAgent, string ip);
        bool isValidToken(int userID, string token);
        int GetUserID(string token);
        void SignOutFromAllDevices(int userId);
        void AddLog(string accessToken, bool isAuthorized, string url, string ip);
        List<string> GetTokenList(int UserID);

    }
}
