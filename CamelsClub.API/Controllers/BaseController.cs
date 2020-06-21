

using CamelsClub.ViewModels.Enums;
using System.Linq;
using System.Web;
using System.Web.Http;
using CamelsClub.API.Helpers;

using System.Net.Http;
using CamelsClub.Data.Helpers;

namespace CamelsClub.API.Controllers
{
    public class BaseController : ApiController
    {

        public BaseController()
        {
            CultureHelper.CurrentCulture = (int)Language;

        }
        string accessTokenHeaderName = "token";
        public string UserID =>  GetUserId();
        public string AccessToken => GetAccessToken();
        public Languages Language => GetLanguageId();

        #region Helpers
        private string GetAccessToken()
        {
            string accessToken = "";
            if (Request.IsLocal() && !HttpContext.Current.Request.Headers.AllKeys.Any(header => header == accessTokenHeaderName))
                accessToken = "";
            else if (HttpContext.Current.Request.Headers.AllKeys.Any(header => header == accessTokenHeaderName))
            {
                accessToken = HttpContext.Current.Request.Headers.GetValues(accessTokenHeaderName).First();
               
            }
            return accessToken;

        }
        private string GetUserId()
        {
            string accessToken = "";
            if (!HttpContext.Current.Request.Headers.AllKeys.Any(header => header == accessTokenHeaderName))
                accessToken = "";
            if (HttpContext.Current.Request.Headers.AllKeys.Any(header => header == accessTokenHeaderName))
            {
                accessToken = HttpContext.Current.Request.Headers.GetValues(accessTokenHeaderName).First();

            }
            return SecurityHelper.GetUserIDFromToken(accessToken).ToString();

        }

        private Languages GetLanguageId()
        {
            try
            {
                if (HttpContext.Current.Request.Headers.AllKeys.Any(header => header == "language"))
                {
                    if (HttpContext.Current.Request.Headers.GetValues("language").First().ToLower().Trim() == "ar")
                        return Languages.Arabic;
                    else
                        return Languages.English;
                }
                else
                    return Languages.Arabic;

            }
            catch
            {
                return Languages.Arabic;
            }

        }

        #endregion

    }
}
