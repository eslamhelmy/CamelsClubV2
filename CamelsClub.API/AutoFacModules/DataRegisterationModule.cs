using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using CamelsClub.Data.Context;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Repositories;

namespace CamelsClub.API.AutoFacModules
{
    public class DataRegisterationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                                  .InstancePerRequest();
            builder.RegisterType<CamelsClubContext>()
                .As<CamelsClubContext>()
                                  .InstancePerRequest();
                                  //.InstancePerLifetimeScope();


        }
    }
}