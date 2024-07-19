using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

class MyService(
  ILogger<MyService> _logger,
  IConfiguration _configuration
)
{
    public void Execute(CancellationToken stoppingToken = default)
    {
        Console.WriteLine("Testing ELK Log");
        
        var xpto = _configuration.GetSection("ElasticApm");
        _logger.LogInformation($"Fleet Server at: {xpto["ServerUrl"]}");

        for (int i = 0; i < 100; i++)
        {
            var randomNumber = new Random().Next(0, 1000);
            _logger.LogInformation($"[Dotnet] Logging Random Number: {randomNumber}", randomNumber);
            Thread.Sleep(1000);
        }

        Console.WriteLine("Goodbye!");
    }
}
