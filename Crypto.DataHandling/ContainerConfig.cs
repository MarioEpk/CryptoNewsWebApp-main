using Autofac;
using Microsoft.EntityFrameworkCore;
using Reddit;
using AutofacSerilogIntegration;
using Crypto.DataHandling.APIClients;
using Serilog;
using Crypto.Models.Data;
using System.IO;

namespace Crypto.DataHandling
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "log.txt"))
                .MinimumLevel.Debug()
                .CreateLogger();

            builder.RegisterLogger();

            builder.Register(x =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
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
