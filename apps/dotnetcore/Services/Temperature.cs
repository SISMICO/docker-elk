namespace dotnetcore.Services;

public interface ITemperature {
  int GetTemperature();
}

public class Temperature : ITemperature
{
  public int GetTemperature() {
    var temp = Random.Shared.Next(-20, 55);
    return temp;
  }
  
  public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<ITemperature, Temperature>();

    services.AddControllersWithViews();
}
}
