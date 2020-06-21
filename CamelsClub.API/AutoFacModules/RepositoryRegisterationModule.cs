using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using CamelsClub.Repositories;

namespace CamelsClub.API.AutoFacModules
{
    public class RepositoryRegisterationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(UserRepository).Assembly)
                      .Where(t => t.Name.EndsWith("Repository"))
                      .AsImplementedInterfaces()
                                        .InstancePerRequest();


        }
    }
}