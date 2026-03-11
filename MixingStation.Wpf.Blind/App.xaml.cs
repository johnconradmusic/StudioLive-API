using Microsoft.Extensions.DependencyInjection;
using MixingStation.Api;
using MixingStation.Api.Helpers;
using MixingStation.Api.Models;
using MixingStation.Api.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MixingStation.Wpf.Blind
{
	public partial class App : Application
	{
		public static ServiceProvider ServiceProvider { get; set; }
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

#if DEBUG
			//Log.Logger = new LoggerConfiguration()
			//	.WriteTo.Console()
			//	.CreateLogger();

			AllocConsole();
#endif

			var serviceCollection = new ServiceCollection();

			// Register the new classes and their dependencies
			serviceCollection.AddSingleton<MixerState>();
			serviceCollection.AddSingleton<MixingStationStateTraverser>();
			serviceCollection.AddSingleton<MixerStateSynchronizer>();
			serviceCollection.AddSingleton<MixerStateService>();

			serviceCollection.AddSingleton<BroadcastService>();
			serviceCollection.AddSingleton<CommunicationService>();
			serviceCollection.AddSingleton<MeterService>();


			// WPF UI
			serviceCollection.AddSingleton<Speech.SpeechManager>();
			serviceCollection.AddSingleton<BlindViewModel>();
			serviceCollection.AddSingleton<MainWindow>();

			ServiceProvider = serviceCollection.BuildServiceProvider();

			ServiceProvider.GetRequiredService<BroadcastService>().StartReceive();
			
			while (!Mixer.Counted)
			{
				Task.Delay(100).Wait();
			}

			// Run application:
			var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
			mainWindow.Show();
		}

		

		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		internal static extern bool AllocConsole();
	}
}
