using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using CamelsClub.Services;

namespace CamelsClub.API.AutoFacModules
{
    public class ServiceRegisterationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(UserService).Assembly)
                  .Where(t => t.Name.EndsWith("Service"))
                  .AsImplementedInterfaces()
                  .InstancePerRequest();

        }
    }
}