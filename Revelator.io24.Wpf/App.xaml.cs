using Microsoft.Extensions.DependencyInjection;
using Presonus.UCNet.Api.Helpers;
using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Api.Services;
using Presonus.UCNet.Api;
using System.Windows;
using Presonus.StudioLive32.Api.Services;
using Serilog;

namespace Presonus.StudioLive32.Wpf
{


	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

#if DEBUG
			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console()
				.CreateLogger();

			AllocConsole();
#endif

			var serviceCollection = new ServiceCollection();

			// Register the new classes and their dependencies
			serviceCollection.AddSingleton<MixerState>();
			serviceCollection.AddSingleton<MixerStateTraverser>();
			serviceCollection.AddSingleton<MixerStateSynchronizer>();
			serviceCollection.AddSingleton<MixerStateService>();

			serviceCollection.AddSingleton<BroadcastService>();
			serviceCollection.AddSingleton<CommunicationService>();
			serviceCollection.AddSingleton<MeterService>();

			// Device model
			//serviceCollection.AddSingleton<StudioLive32R>();

			// WPF UI
			serviceCollection.AddSingleton<MainViewModel>();
			serviceCollection.AddSingleton<MixerWindow>();

			var serviceProvider = serviceCollection.BuildServiceProvider();

			serviceProvider.GetRequiredService<BroadcastService>().StartReceive();

			// Run application:
			var mainWindow = serviceProvider.GetRequiredService<MixerWindow>();
			mainWindow.Show();
		}

		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		internal static extern bool AllocConsole();
	}

}
