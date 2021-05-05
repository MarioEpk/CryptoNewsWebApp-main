using Autofac;
using CryptoNewsWebApp.Data;
using DataAPIRequests.APIClients;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Reddit;

namespace DataAPIRequests
{
    public static class ContainerConfig
    {

        
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.Register(x =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-CryptoNewsWebApp-322651FA-D6A9-4E1E-83BA-2F51C589D8E3;Trusted_Connection=True;MultipleActiveResultSets=true");
                return new ApplicationDbContext(optionsBuilder.Options);
            }).InstancePerLifetimeScope();

            builder.RegisterType<ApplicationDbContext>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<BusinessLogic>().As<IBusinessLogic>();
            builder.RegisterType<CardanoRedditClient>().As<IDataAccess>();
            builder.RegisterType<RedditClient>().AsSelf();


            //builder.registertype<applicationdbcontext>().asself();

            

            return builder.Build();
        }

    }
}
