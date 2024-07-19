using Microsoft.EntityFrameworkCore;
using dotnetcore.Models;

public class ApplicationDbContext : DbContext
{
  public DbSet<WeatherForecast>? Weathers { get; set; }

  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
  {

  }
}
