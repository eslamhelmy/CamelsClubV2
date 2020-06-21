using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

using System.Web.Mvc;
using System.Web.Http;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using CamelsClub.Services;
using CamelsClub.Data.Context;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Repositories;
using System.Configuration;
using CamelsClub.API.AutoFacModules;
using CamelsClub.API.Filters;

namespace CamelsClub.API.App_Start
{
    public class Bootstrapper : Autofac.Module
    {

        public static void Run()
        {
            SetAutofacContainer();
        }
        private static void SetAutofacContainer()
        {
            try
            {
                var builder = new ContainerBuilder();
                builder.RegisterModule<ControllersRegisterationModule>();
                builder.RegisterModule<DataRegisterationModule>();
                builder.RegisterModule<RepositoryRegisterationModule>();
                builder.RegisterModule<ServiceRegisterationModule>();
                //for register filter
                //////////////
                builder.RegisterFilterProvider();
                builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

               // builder
                 //   .RegisterType<AuthorizeUserFilterTestAttribute>().PropertiesAutowired().InstancePerRequest();
                /////////////////
            
                IContainer container = builder.Build();
                //for overriding the main Dependency resolver
                DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
                GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            }
            catch (System.Exception e)
            {

               
            }
        }
    }
}