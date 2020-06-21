using CamelsClub.Models;
using CamelsClub.Services.Helpers;
using CamelsClub.Services;
using System;
using System.Linq;
using CamelsClub.Data.Helpers;
using System.Collections.Generic;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Repositories;
using CamelsClub.ViewModels;

namespace CamelsClub.Services
{
    public class TokenService : ITokenService
    {
        private readonly IUnitOfWork _unit;
        private readonly ITokenRepository _repo;
        public TokenService(IUnitOfWork unit, ITokenRepository repo)
        {
            _unit = unit;
            _repo = repo;
        }
        //public TokenService(IUnitOfWork unit)
        //{
        //    _unit = unit;

        //}


        public TokenViewModel AddToken(string token, string userAgent, string ip)
        {
            int userID = SecurityHelper.GetUserIDFromToken(token);
            if (!string.IsNullOrEmpty(token))
            {
                token = SecurityHelper.Encrypt(token);
                Token newToken = new Token() { UserID = userID, Active = true, IP = HttpRequestHelper.GetClientIP(), TokenGUID = token, ExpireDate = DateTime.Now.AddDays(100), UserAgent = userAgent };
                return _repo.Add(newToken).ToViewModel();
            }
            else
            {
                return null;
            }

        }
        public TokenViewModel AddTokenForMobile(string token, string deviceID, string userAgent, string ip)
        {
            int userID = SecurityHelper.GetUserIDFromToken(token);
            if (!string.IsNullOrEmpty(token))
            {
                token = SecurityHelper.Encrypt(token);
                Token newToken = new Token() { UserID = userID, DeviceID = deviceID, Active = true, IP = HttpRequestHelper.GetClientIP(), TokenGUID = token, ExpireDate = DateTime.Now.AddDays(100), UserAgent = userAgent };
                return _repo.Add(newToken).ToViewModel();
            }
            else
            {
                return null;
            }
        }

        public int GetUserID(string token)
        {
            return _repo.GetAll().Where(t => t.TokenGUID == token).Select(t => t.UserID).FirstOrDefault();

        }

        public void SignOutFromAllDevices(int userId)
        {
            DateTime currentDateTime = DateTime.Now;

            var tokensIDs = _repo.GetAll().Where(accessToken => !accessToken.IsDeleted && accessToken.UserID == userId && !accessToken.LoggedOutDate.HasValue).Select(t => t.ID);
            foreach (var item in tokensIDs)
            {
                _repo.SaveIncluded(new Token
                {
                    ID = item,
                    LoggedOutDate = DateTime.Now
                }, "LoggedOutDate");
            }
        }
        public bool isValidToken(int userID, string token)
        {
            DateTime currentDateTime = DateTime.Now;
            var tkn = _repo.GetAll().Where(t =>!t.IsDeleted&& t.TokenGUID == token && t.LoggedOutDate == null).FirstOrDefault();
            if (tkn != null)
                return true;
            else
                return false;

        }

        public void AddLog(string accessToken, bool isAuthorized, string url, string ip)
        {
            var existToken = _repo.GetAll().FirstOrDefault(token => !token.IsDeleted && token.TokenGUID == accessToken);
            if (existToken != null)
            {
                existToken.TokenLogs.Add(new TokenLog() { IsAuthorized = isAuthorized, URL = url, IP = CamelsClub.Services.Helpers.HttpRequestHelper.GetClientIP() });
            }
        }

        //public void UpdateConnectionID(string token, string connectionID)
        //{
        //    token = SecurityHelper.Encrypt(token);
        //    var existToken = _repo.GetAll().FirstOrDefault(t => !t.IsDeleted && t.TokenGUID == token);
        //    if (existToken != null)
        //    {
        //        existToken.SignalRConnectionID = connectionID;
        //        _repo.Edit(existToken);
        //    }
        //}

        public NotificationReceiverTokensViewModels GetConnectionIDsAndDevicesIDs(int userID)
        {
            var query = _repo.GetAll().Where(c => c.UserID == userID).Select(c => new NotificationReceiverTokensViewModels
            {
                DevicesIDs = c.User.Tokens.Where(t => t.DeviceID != null && t.ExpireDate > DateTime.Now && t.LoggedOutDate == null).OrderBy(t => t.ID).Select(t => t.DeviceID).Distinct(),
               //ConnectionIDs = c.User.Tokens.Where(t => t.SignalRConnectionID != null && t.ExpirationDate > DateTime.Now && t.LoggedOutDate == null).Select(t => t.SignalRConnectionID)
            });

            return query.FirstOrDefault();
        }
        public NotificationReceiverTokensViewModels GetConnectionIDsAndDevicesIDs(List<int> usersIDs)
        {
            var query = _repo.GetAll().Where(c => usersIDs.Contains(c.UserID) && !c.IsDeleted).Select(c => new NotificationReceiverTokensViewModels
            {
                DevicesIDs = c.User.Tokens.Where(t => t.DeviceID != null && t.ExpireDate > DateTime.Now && t.LoggedOutDate == null).OrderBy(t => t.ID).Select(t => t.DeviceID).Distinct(),
                //ConnectionIDs = c.User.Tokens.Where(t => t.SignalRConnectionID != null && t.ExpirationDate > DateTime.Now && t.LoggedOutDate == null).Select(t => t.SignalRConnectionID)
            });

            return query.FirstOrDefault();
        }
        public NotificationReceiverTokensViewModels GetConnectionIDsAndDevicesIDsByUserName(string userName)
        {
            userName = SecurityHelper.Encrypt(userName);
            var query = _repo.GetAll().Where(c => c.User.UserName == userName).Select(c => new NotificationReceiverTokensViewModels
            {
                DevicesIDs = c.User.Tokens.Where(t => t.DeviceID != null && t.ExpireDate > DateTime.Now && t.LoggedOutDate == null).OrderBy(t => t.ID).Select(t => t.DeviceID).Distinct(),
                //ConnectionIDs = c.User.Tokens.Where(t => t.SignalRConnectionID != null && t.ExpirationDate > DateTime.Now && t.LoggedOutDate == null).Select(t => t.SignalRConnectionID)
            });

            return query.FirstOrDefault();
        }
     


        public List<string> GetTokenList(int UserID)
        {
            return _repo.GetAll().Where(t => t.UserID == UserID && t.ExpireDate > DateTime.Now && t.LoggedOutDate == null && t.DeviceID != null).OrderByDescending(t => t.ID).Select(t => t.DeviceID).Distinct().ToList();
        }



    }
}
