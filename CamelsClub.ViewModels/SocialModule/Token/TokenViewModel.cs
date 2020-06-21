using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class TokenViewModel
    {
        public int UserID { get; set; }
        public string TokenGUID { get; set; }
        public string IP { get; set; }
        public string UserAgent { get; set; }
        public string DeviceID { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime? LoggedOutDate { get; set; }
    }
    public static class TokenExtension
    {
        public static TokenViewModel ToViewModel(this Models.Token model)
        {
            return new TokenViewModel
            {
                TokenGUID = model.TokenGUID,
                DeviceID = model.DeviceID,
                ExpirationDate = model.ExpireDate,
                IP = model.IP,
                UserID = model.UserID,
                UserAgent = model.UserAgent
            };
        }
    }
}
