using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Presonus.StudioLive32.Api;
using Presonus.StudioLive32.Api.Configuration;
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
            serviceCollection.AddRevelatorAPI();

            
            serviceCollection.AddSingleton<Mixer>();
            serviceCollection.AddSingleton<MainViewModel>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            serviceProvider.StartRevelatorAPI();
            
            //Run application:
            var mainWindow = serviceProvider.GetRequiredService<Mixer>();
            mainWindow.Show();
            Log.Information("Application ready.");
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        internal static extern bool AllocConsole();



    }
}
