using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EliteAPI;
using EliteAPI.Events;
using Hyperlink.Core.Streaming;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hyperlink.Worker.Game
{
    public class GameWorker : BackgroundService
    {
        private ILogger<GameWorker> Logger { get; }
        private EliteDangerousAPI Api { get; }
        private IEliteStream EliteStream { get; }

        public GameWorker(ILogger<GameWorker> logger, IEliteStream eliteStream)
        {
            Logger = logger;
            EliteStream = eliteStream;
            Api = new EliteDangerousAPI(GetEdPath());
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Api.OnReady += OnReady;

            Api.Start();
            Api.Events.StatusHardpointsEvent += HardpointEventHandler;

            return base.StartAsync(cancellationToken);
        }

        private void HardpointEventHandler(object sender, StatusEvent e)
        {
            Logger.LogDebug(Newtonsoft.Json.JsonConvert.SerializeObject(e, Newtonsoft.Json.Formatting.Indented));
            EliteStream.Publish(new EliteMessage
            {
                Hardpoint = (bool)e.Value,
            });
        }

        private void OnReady(object sender, EventArgs e)
        {
            Logger.LogInformation("Commander {Commander} ready!", Api.Commander.Commander);
            Logger.LogInformation("Startup hardpoint status: {status}", Api.Status.Hardpoints);
            EliteStream.Publish(new EliteMessage
            {
                Hardpoint = Api.Status.Hardpoints,
            });
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        private DirectoryInfo GetEdPath()
        {
            var path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Saved Games",
                "Frontier Developments",
                "Elite Dangerous");
            return new DirectoryInfo(path);
        }
    }
}