using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Models;
using CamelsClub.Repositories;
using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CamelsClub.Services
{
    public class UserConfirmationMessageService : IUserConfirmationMessageService
    {
        private readonly IUnitOfWork _unit;
        private readonly IUserConfirmationMessageRepository _repo;
        public UserConfirmationMessageService(IUnitOfWork unit, IUserConfirmationMessageRepository repo)
        {
            _unit = unit;
            _repo = repo;
        }


        public void InsertUserConfirmationMessage(int userID, string code)
        {
            _repo.Add(new UserConfirmationMessage { UserID = userID, Code = code, IsDeleted = false, CreatedBy = userID.ToString(), CreatedDate = DateTime.UtcNow });
        }


        public string ResendCode(string phone)
        {
            string code = _repo.GetNotUsedCode(phone);
             if (string.IsNullOrWhiteSpace(code))
            {
                throw new Exception(Resource.CodeUsedBefore);
            }
            return code;
        }

        public ConfirmationMessageViewModel UpdateUserConfirmationMessage(string code, int userID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException(Resource.InvaildCode);
            var IsValid = _repo.IsValidCode(code, userID);
            if (!IsValid)
                throw new Exception(Resource.InvaildCode);

            var confirmMessage = _repo.GetUserConfirmMessage(code, userID);
            if(confirmMessage == null)
                throw new Exception(Resource.InvaildCode);
            
            confirmMessage.IsUsed = true;
            confirmMessage.UpdatedBy = confirmMessage.UserID.ToString();
            confirmMessage.UpdateDate = DateTime.UtcNow;

            
            return new ConfirmationMessageViewModel
            {
                UserID = confirmMessage.UserID,
                Code = confirmMessage.Code,
                UserName = confirmMessage.User?.UserName,
              //  Phone = confirmMessage.User?.Phone,
                ProfileImagePath = $"{protocol}://{hostName}/uploads/User-Document/{confirmMessage.User?.UserProfile?.MainImage}"
                
            };

          

        }

    }
}

