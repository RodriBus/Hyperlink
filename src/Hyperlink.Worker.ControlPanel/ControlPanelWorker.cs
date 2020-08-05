using Hyperlink.Core.Streaming;
using Hyperlink.Worker.ControlPanel.Messages;
using Hyperlink.Worker.ControlPanel.Messages.Device;
using Hyperlink.Worker.ControlPanel.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;

using System.Threading.Tasks;

namespace Hyperlink.Worker.ControlPanel
{
    public class ControlPanelWorker : BackgroundService
    {
        private ILogger<ControlPanelWorker> Logger { get; }
        private ISerialService Serial { get; }
        private IEliteStream EliteStream { get; }

        public ControlPanelWorker(ILogger<ControlPanelWorker> logger, ISerialService serial, IEliteStream eliteStream)
        {
            Logger = logger;
            Serial = serial;
            EliteStream = eliteStream;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Serial.Configure()
                .OnProcessData(msg => Logger.LogDebug(msg))
                .OnProcessData(HandleDeviceMessage)
                .Open();

            EliteStream.Subscribe(msg => SendHardpointStatus(msg.Hardpoint));

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        private void HandleDeviceMessage(string message)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var msg = JsonSerializer.Deserialize<StatusMessage>(message, options);
            Logger.LogInformation("Device status: {Status}", msg.Status);
        }

        private void SendHardpointStatus(bool status)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var msg = new HardpointStatusMessage
            {
                Hardpoint = status,
            };
            var str = JsonSerializer.Serialize(msg, options);
            Logger.LogInformation(str);
            Serial.Write(str);
        }
    }
}