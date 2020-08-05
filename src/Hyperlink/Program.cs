using Hyperlink.Core;
using Hyperlink.Worker.ControlPanel;
using Hyperlink.Worker.ControlPanel.Extensions;
using Hyperlink.Worker.Game;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Hyperlink
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(services);

                    services.AddCoreServices();

                    services.AddControlPanelServices();

                    services.AddHostedService<GameWorker>();
                    services.AddHostedService<ControlPanelWorker>();
                });
    }
}