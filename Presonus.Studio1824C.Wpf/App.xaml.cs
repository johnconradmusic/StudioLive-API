using Microsoft.Extensions.DependencyInjection;
using Presonus.StudioLive32.Api;
using Presonus.StudioLive32.Api.Configuration;
using Presonus.StudioLive32.Api.Services;
using Presonus.UC.Api.Devices;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.IO;
using System.Windows;

namespace Presonus.Studio1824C.Wpf
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
			//serviceCollection.AddUCNetAPI();
			serviceCollection.AddSingleton<BroadcastService>();
			serviceCollection.AddSingleton<CommunicationService>();
			serviceCollection.AddSingleton<MonitorService>();

			//API:
			serviceCollection.AddSingleton<RawService>();
			//serviceCollection.AddSingleton<StudioLive32R>();
			serviceCollection.AddSingleton<Presonus.UC.Api.Devices.Studio1824C>(); //Device
			serviceCollection.AddSingleton<Mixer1824>(); //window
			serviceCollection.AddSingleton<MainViewModel>(); //viewmodel

			var serviceProvider = serviceCollection.BuildServiceProvider();
			//serviceProvider.GetRequiredService<RawService>().JSON();
			serviceProvider
				.GetRequiredService<BroadcastService>()
				.StartReceive();
			// serviceProvider.StartRevelatorAPI();

			//Run application:
			var mainWindow = serviceProvider.GetRequiredService<Mixer1824>();
			mainWindow.Show();
		}

		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		internal static extern bool AllocConsole();



	}
}
