using Microsoft.AspNetCore.Mvc;
using dotnetcore.Services;
using dotnetcore.Models;

namespace dotnetcore.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
  private static readonly string[] Summaries = new[]
  {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

  private readonly ILogger<WeatherForecastController> _logger;
  private readonly ITemperature _temperatureService;
  private readonly ApplicationDbContext _context;
  
  private readonly Random _random = new();

  public WeatherForecastController(ILogger<WeatherForecastController> logger, ApplicationDbContext context, ITemperature temperatureService)
  {
    _logger = logger;
    _temperatureService = temperatureService;
    _context = context;
  }

  [HttpGet(Name = "GetWeatherForecast")]
  public IEnumerable<WeatherForecast> Get()
  {
    this._logger.LogInformation("Passei por aqui! {Number}", _random.Next(1, 1000));

    int randomNumber = _random.Next(1, 3);
    this._logger.LogInformation($"Random Number: {randomNumber}", randomNumber);
    if (randomNumber == 1)
      throw new Exception($"Exception Test: {_random.Next(1001, 2000)}");

    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
      Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
      TemperatureC = _temperatureService.GetTemperature(),
      Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    })
    .ToArray();
  }

  [HttpPost(Name = "PostWeatherForecast")]
  public WeatherForecast Process()
  {
    var weather = new WeatherForecast
    {
      Date = DateOnly.FromDateTime(DateTime.Now),
      TemperatureC = _temperatureService.GetTemperature(),
      Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    };

    this._context.Add(weather);
    this._context.SaveChanges();

    return weather;
  }
}
