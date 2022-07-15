using Microsoft.Extensions.DependencyInjection;
using Presonus.StudioLive32.Api.Services;
using Presonus.UC.Api.Devices;
using System;

namespace Presonus.StudioLive32.Api.Configuration
{
	public static class ServiceProviderExtension
    {
        public static void AddUCNetAPI(this IServiceCollection serviceCollection)
        {
            //Services:
            serviceCollection.AddSingleton<BroadcastService>();
            serviceCollection.AddSingleton<CommunicationService>();
            serviceCollection.AddSingleton<MonitorService>();
                        
            //API:
            serviceCollection.AddSingleton<RawService>();
            serviceCollection.AddSingleton<StudioLive32R>();
        }

        public static void StartRevelatorAPI(this IServiceProvider serviceProvider)
        {
            serviceProvider
                .GetRequiredService<BroadcastService>()
                .StartReceive();
        }
    }
}
