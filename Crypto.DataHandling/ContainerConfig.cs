using Autofac;
using Crypto.WebApplication.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Reddit;
using AutofacSerilogIntegration;
using Crypto.DataHandling.APIClients;
using Serilog;

namespace Crypto.DataHandling
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(@"C:\Users\Epakf\source\repos\CryptoNewsWebApp-main\Crypto.DataHandling\log.txt")
                .MinimumLevel.Debug()
                .CreateLogger();

            builder.RegisterLogger();

            builder.Register(x =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-CryptoNewsWebApp-322651FA-D6A9-4E1E-83BA-2F51C589D8E3;Trusted_Connection=True;MultipleActiveResultSets=true");
                return new ApplicationDbContext(optionsBuilder.Options);
            }).InstancePerLifetimeScope();

            builder.RegisterType<BusinessLogic>().As<IBusinessLogic>();
            builder.RegisterType<CardanoRedditClient>();
            builder.RegisterType<MarketCapClient>();

            builder.RegisterType<RedditClient>().AsSelf();



            return builder.Build();
        }

    }
}
