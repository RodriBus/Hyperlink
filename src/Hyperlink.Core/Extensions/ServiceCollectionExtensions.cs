using Hyperlink.Core.Streaming;
using Microsoft.Extensions.DependencyInjection;

namespace Hyperlink.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services) => services
                .AddSingleton<IEliteStream, EliteStream>();
    }
}