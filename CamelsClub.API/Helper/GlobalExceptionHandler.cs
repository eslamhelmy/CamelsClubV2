using CamelsClub.Data.UnitOfWork;
using CamelsClub.Services.Helpers;
using CamelsClub.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace CamelsClub.API.Helpers
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
       // public readonly IApplicationLogService _applicationLogService;
       // public readonly IUnitOfWork _unitOfWork;
        public GlobalExceptionHandler()
        {

        }
        public override void Handle(ExceptionHandlerContext context)
        {
            var resultViewModel = new ResultViewModel<string>();
          //  resultViewModel.Message = "Error Occurred";
            var exception = context.Exception;
            string otherInfo = "";
            if (exception != null)
            {
                if(exception.InnerException != null)
                {
                    while (exception.InnerException != null) exception = exception.InnerException;
                    otherInfo = "Message: " + exception.Message + " StackTrace :" + exception.StackTrace + " Sourcec: " + exception.Source ?? "";

                }
                else
                {
                    var res = context.Request.CreateResponse(HttpStatusCode.BadRequest,
                        new ResultViewModel<string>(exception.Message));
                    context.Result = new ResponseMessageResult(res);

                    return ;

                }

            }
            ApplicationLogCreateViewModel log = new ApplicationLogCreateViewModel();
            log.Data = "";
            log.Description = otherInfo;
            log.IP = "IP";
            log.LogTypeID = 1;
            log.Title = context.Exception.Message;
            log.URL = context.Request.RequestUri.AbsoluteUri;
            ApplicationLogService.AddApplicationLog(log);
            resultViewModel.Data = otherInfo;
          //  ApplicationLogService.NotifyMe(log);
            var response = context.Request.CreateResponse(HttpStatusCode.OK, resultViewModel);
            context.Result = new ResponseMessageResult(response);


        }


    }
}