using System.Threading.Tasks;
using Autofac;

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
