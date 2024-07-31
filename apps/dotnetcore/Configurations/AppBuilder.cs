using dotnetcore.Services;
using Elastic.Apm.SerilogEnricher;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Templates;

public class AppBuilder
{
  public static WebApplication createBuilder(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);
    
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithElasticApmCorrelationInfo()
        .Enrich.WithEnvironmentName()
        .Enrich.WithAppVersion()
        .WriteTo.Console(new RehagroLogFormatter())
        .WriteTo.Http(requestUri: "http://localhost:8080", queueLimitBytes: null, textFormatter: new RehagroLogFormatter())
        .CreateLogger();

    builder.Host.UseSerilog(logger);
    // builder.Services.AddAllElasticApm();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSingleton<ITemperature, Temperature>();
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLDatabase"))
    );

    return builder.Build();
  }
}


