using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;

namespace Test_Analytics.Web {
    public class Program {
        public static void Main( string[] args ) {
            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft",LogEventLevel.Information)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.File(Path.Combine("logs", @"logs.txt"), rollingInterval: RollingInterval.Day ).CreateLogger();

            var host = CreateWebHostBuilder( args ).Build();

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder( string[] args ) =>
            WebHost.CreateDefaultBuilder( args )
                .UseStartup<Startup>()
                .UseSerilog();
    }
}
