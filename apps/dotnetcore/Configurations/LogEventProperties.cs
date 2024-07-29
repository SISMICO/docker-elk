using System.Reflection;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

public static class LogEventProperties {
  public static LoggerConfiguration WithAppVersion(
    this LoggerEnrichmentConfiguration enrichmentConfiguration
  ) {
    if (enrichmentConfiguration == null) {
      throw new ArgumentNullException(nameof(enrichmentConfiguration));
    }
    
    return enrichmentConfiguration.With<AppVersionEnricher>();
  }
}

public class AppVersionEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var appVersion = Assembly.GetEntryAssembly()?.GetName().Version?.ToString();
        appVersion ??= "1.0.0";
        var property = new LogEventProperty("AppVersion", new ScalarValue(appVersion));
        logEvent.AddPropertyIfAbsent(property);
    }
}
