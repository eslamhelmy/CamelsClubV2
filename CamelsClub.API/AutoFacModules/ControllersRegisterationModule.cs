using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using CamelsClub.Services;

namespace CamelsClub.API.AutoFacModules
{
    public class ControllersRegisterationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired().InstancePerRequest();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired().InstancePerRequest();


        }
    }
}