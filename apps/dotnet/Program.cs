using Serilog;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace dotnet;

class Program
{
    static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddJsonFile("appsettings.json")
        .Build();
        
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            // .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.Http(requestUri: "http://localhost:8080", queueLimitBytes: null)
            .CreateLogger();
        
        var builder = Host.CreateApplicationBuilder(args);
        builder.Configuration.AddConfiguration(configuration);
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();
        ConfigureServices(builder.Services);
        // .ConfigureAppConfiguration((config) => config.AddConfiguration(configuration))
        var host = builder.Build();

        var myService = host.Services.GetRequiredService<MyService>();
        myService.Execute();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // services.AddAllElasticApm();
        services.AddTransient<MyService>();
        // services.AddLogging(loggerBuilder => loggerBuilder.AddSerilog());
    }
}
