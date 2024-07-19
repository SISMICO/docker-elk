using dotnetcore.Services;
using Elastic.Apm.SerilogEnricher;
using Microsoft.EntityFrameworkCore;
using Serilog;

public class AppBuilder
{
  public static WebApplication createBuilder(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);
    
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

    builder.Host.UseSerilog(logger);
    builder.Services.AddAllElasticApm();

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


