using CamelsClub.API.Helpers;
using Microsoft.Web.Http;
using Microsoft.Web.Http.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using CamelsClub.API.Filters;
namespace CamelsClub.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.AddApiVersioning(cfg => {
                cfg.DefaultApiVersion = new ApiVersion(1, 0);
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.ApiVersionReader = new HeaderApiVersionReader("X-version");
                cfg.ReportApiVersions = true;
            });
            config.MapHttpAttributeRoutes();
            //EnableCorsAttribute cors = new EnableCorsAttribute("*", "*",
            //                                   "GET, PUT, POST, DELETE, OPTIONS");
           // config.EnableCors(cors);
          //  config.EnableCors();
           // config.Filters.Add(new ValidateViewModelAttribute());
         //   config.Services.Replace(typeof(IExceptionLogger), new GlobalExceptionHandler());
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }

            //constraints: new string[] { "Infinite.API.Controllers" }
            //namespaces: new string[] { "Infinite.API.Controllers" }


            );

            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
        }
    }
}
