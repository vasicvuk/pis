using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Graylog.Core;
using Serilog.Sinks.Graylog.Core.Transport;

namespace PSD2Payment
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var centralLogHost = Environment.GetEnvironmentVariable("CENTRAL_LOG_HOST"); 
            var loggerConfig = new LoggerConfiguration()
             .MinimumLevel.Debug()
             .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
             .Enrich.FromLogContext()
            .WriteTo.Console();
            if (centralLogHost != null && !centralLogHost.Equals(""))
            {
                var centralLogPort = Environment.GetEnvironmentVariable("CENTRAL_LOG_PORT");
                var centralLogProtocol = Environment.GetEnvironmentVariable("CENTRAL_LOG_PROTOCOL");
                TransportType transportType = TransportType.Http;
                if (centralLogProtocol != null && centralLogProtocol.Equals("Udp"))
                {
                    transportType = TransportType.Udp;
                }
                loggerConfig.WriteTo.Graylog(new GraylogSinkOptions
                {
                    Port = int.Parse(centralLogPort),
                    HostnameOrAddress = centralLogHost,
                    TransportType = transportType
                });
            }
            Log.Logger = loggerConfig.CreateLogger();
            try
            {
                Log.Information("Starting web host");
                CreateWebHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseStartup<Startup>();
    }
}
