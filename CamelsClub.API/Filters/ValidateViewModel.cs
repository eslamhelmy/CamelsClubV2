
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CamelsClub.API.Helpers;
using CamelsClub.ViewModels;


namespace CamelsClub.API.Filters
{
    public class ValidateViewModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var resultViewModel = new ResultViewModel<string>();
                resultViewModel.Success = false;
                resultViewModel.Data = string.Join("; ", actionContext.ModelState.Values
                                                                                       .SelectMany(modelState => modelState.Errors)
                                                                                       .Select(error => error.ErrorMessage));
                //new List<object>();
                resultViewModel.Message = string.Join("; ", actionContext.ModelState.Values
                                                                                       .SelectMany(modelState => modelState.Errors)
                                                                                       .Select(error => error.ErrorMessage));
                resultViewModel.Errors = APIHelper.ValidationMessages(actionContext.ModelState);
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                                                                    HttpStatusCode.BadRequest,
                                                                    resultViewModel.Data);
                //actionContext.Response = actionContext.Request.CreateResponse(
                //                                                   HttpStatusCode.ExpectationFailed,
                //                                                   resultViewModel,
                //                                                   actionContext.ControllerContext.Configuration.Formatters.JsonFormatter);

            }
        }
    }
}