using System;
using System.IO;
using System.Threading.Tasks;
using CuteServiceBusExplorer.Cli.Commands;
using CuteServiceBusExplorer.Cli.Temp_Mock;
using CuteServiceBusExplorer.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace CuteServiceBusExplorer.Cli
{
    public static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"), optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();    
            
            var serilog = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(config =>
                    {
                        config.ClearProviders();
                        config.AddProvider(new SerilogLoggerProvider(serilog));
                        var minimumLevel = configuration.GetSection("Serilog:MinimumLevel")?.Value;
                        if (!string.IsNullOrEmpty(minimumLevel))
                        {
                            config.SetMinimumLevel(Enum.Parse<LogLevel>(minimumLevel));                            
                        }  
                    });
                    services.AddTransient<IConnectionService, Temp_Mock.ConnectionService>();
                    services.AddTransient<ITopicService, Temp_Mock.TopicService>();
                });
            
            try
            {
                return await builder.RunCommandLineApplicationAsync<Root>(args);
            }
            catch(Exception ex)
            {
                serilog.Fatal(ex, "Uncaught exception");
                return (int)ExitCodes.GeneralError;
            }
        }
    }
}