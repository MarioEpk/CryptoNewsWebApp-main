using Crypto.WebApplication.Data;
using Crypto.WebApplication.Models;
using Crypto.DataHandling.APIClients;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Autofac;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole;
    
namespace Crypto.DataHandling
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        private static async Task MainAsync()
        {
            // Autofac DI
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IBusinessLogic>();
                await app.ProcessData();
            }
        }
    }
}
