using Hyperlink.Worker.ControlPanel.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hyperlink.Worker.ControlPanel.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddControlPanelServices(this IServiceCollection services) => services
                .AddTransient<IControlPanelDeviceLocator, ControlPanelDeviceLocator>()
                .AddTransient<ISerialService, SerialService>();
    }
}