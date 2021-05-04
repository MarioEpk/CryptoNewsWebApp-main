using CryptoNewsWebApp.Data;
using CryptoNewsWebApp.Models;
using DataAPIRequests.APIClients;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Autofac;

namespace DataAPIRequests
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

            using ( var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IBusinessLogic>();
                await app.ProcessData();
            }

            

            // Teraz zacat zapisovat do database, skusit spravit test ci sa to zapisalo, a skontrolovat ci sa zapisuju len unique server ID
            // potom zistit ako z toho spravit schedule task

            // Prepisat subreddit model, pretoze je to vlastne zbytocne ho aj mat asi, pripadne pridat datum kedy bol tento call spraveny, 
            // + tie ten unique check, + modelisvalidstate 
        }
    }
}
