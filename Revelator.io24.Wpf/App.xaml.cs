using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Presonus.StudioLive32.Api;
using Presonus.StudioLive32.Api.Configuration;
using Presonus.StudioLive32.Api.Models;
using Presonus.StudioLive32.Api.Services;
using Presonus.UC.Api.Devices;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.IO;
using System.Windows;

namespace Presonus.StudioLive32.Wpf
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

#if DEBUG
			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console(theme: ConsoleTheme.None)
				.CreateLogger();

			AllocConsole();
#endif

			var serviceCollection = new ServiceCollection();
			serviceCollection.AddSingleton<BroadcastService>();
			serviceCollection.AddSingleton<CommunicationService>();
			serviceCollection.AddSingleton<MonitorService>();

			//API:
			serviceCollection.AddSingleton<RawService>();
			serviceCollection.AddSingleton<StudioLive32R>();
			//serviceCollection.AddSingleton<Studio1824C>();
			serviceCollection.AddSingleton<Mixer>();
			serviceCollection.AddSingleton<MainViewModel>();

			DeviceRoutingBase.loadingFromScene = true;

			var serviceProvider = serviceCollection.BuildServiceProvider();
			serviceProvider
				.GetRequiredService<BroadcastService>()
				.StartReceive();
			//serviceProvider.GetRequiredService<RawService>().JSON();
			// serviceProvider.StartRevelatorAPI();

			//Run application:
			var mainWindow = serviceProvider.GetRequiredService<Mixer>();
			mainWindow.Show();
			Log.Information("Application ready.");
		}

		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		internal static extern bool AllocConsole();



	}
}
