
//using CamelsClub.Data.Context;
//using CamelsClub.Data.Helpers;
//using CamelsClub.Data.UnitOfWork;
//using CamelsClub.Repositories;
//using CamelsClub.Services;
//using CamelsClub.Services.Helpers;
//using CamelsClub.ViewModels;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web;
//using System.Web.Http;
//using System.Web.Http.Controllers;
//using System.Web.Http.Filters;

//namespace CamelsClub.API.Filters
//{
//    public class AuthorizeUserFilterTestAttribute : ActionFilterAttribute
//    {
      
//        public IUserRoleService UserRoleService { get; set; }
//        public ITokenService TokenService { get; set; }
//        public IUnitOfWork UnitOfWork { get; set; }

//        public string Role { get; set; }

      
//        public override void OnActionExecuting(HttpActionContext actionContext)

//        {
//            // base.OnActionExecuting(actionContext);
//            string iP = HttpContext.Current.Request.UserHostAddress.ToLower();
//            bool allowLocal = false;
//            if (!HttpContext.Current.Request.IsLocal || !allowLocal)
//            {
//                bool isAuthorized = false;
//                string accessToken = "";
//                try
//                {
//                    string accessTokenHeaderName = "token";
//                    if (actionContext.Request.Headers.Any(header => header.Key == accessTokenHeaderName))
//                    {
//                        accessToken = actionContext.Request.Headers.GetValues(accessTokenHeaderName).FirstOrDefault();
//                        if (!SecurityHelper.IsTokenExpired(accessToken))
//                        {
//                            int userID = SecurityHelper.GetUserIDFromToken(accessToken);
//                            // _userRoleService still see it null
//                            if (userID > 0 && UserRoleService.HasRole(userID, Role))//  _userService.HasRole(userID, Role))
//                            {
//                                //_tokenService.UserID = UserID.ToString();
//                                accessToken = SecurityHelper.Encrypt(accessToken);
//                                DateTime currentDateTime = DateTime.Now;
//                                isAuthorized = TokenService.isValidToken(userID, accessToken);

//                            }

//                        }
//                    }



//                }
//                catch (Exception ex)
//                {

//                }
//                TokenService.AddLog(accessToken, isAuthorized, actionContext.Request.RequestUri.AbsoluteUri, actionContext.Request.GetClientIP());
//                UnitOfWork.Save();

//                //isAuthorized = true;
//                if (!isAuthorized)
//                {
//                    var resultViewModel = new ResultViewModel<string>();
//                    resultViewModel.Success = false;
//                    resultViewModel.Message = "Unauthorized";
//                    resultViewModel.Authorized = false;
//                    actionContext.Response = actionContext.Request.CreateResponse(
//                                                                        HttpStatusCode.Unauthorized,
//                                                                        resultViewModel,
//                                                                        actionContext.ControllerContext.Configuration.Formatters.JsonFormatter);
//                    //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
//                }
//            }

//            base.OnActionExecuting(actionContext);
        

//    }
        
//    }
//}