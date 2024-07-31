using System.Globalization;
using System.Text.Json;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;

public class RehagroLogFormatter : ITextFormatter
{
  public void Format(LogEvent logEvent, TextWriter output)
  {
    try
    {
      var buffer = new StringWriter();
      FormatContent(logEvent, buffer);

      // If formatting was successful, write to output
      output.WriteLine(buffer.ToString());
    }
    catch (Exception e)
    {
      LogNonFormattableEvent(logEvent, e);
    }
  }


  private void FormatContent(LogEvent logEvent, TextWriter output)
  {
    if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));
    if (output == null) throw new ArgumentNullException(nameof(output));

    output.Write("{\"Timestamp\":\"");
    output.Write(logEvent.Timestamp.UtcDateTime.ToString("o"));

    output.Write("\",\"Level\":\"");
    output.Write(logEvent.Level);

    output.Write("\",\"Message\":");
    var message = logEvent.MessageTemplate.Render(logEvent.Properties);
    JsonValueFormatter.WriteQuotedJsonString(message, output);

    if (logEvent.Exception != null)
    {
      output.Write(",\"Exception\":");
      JsonValueFormatter.WriteQuotedJsonString(logEvent.Exception.ToString(), output);
    }

    output.Write(",\"ServiceName\":");
    JsonValueFormatter.WriteQuotedJsonString(AppDomain.CurrentDomain.FriendlyName, output);

    WritePropertyValue("ServiceVersion", "AppVersion", logEvent.Properties, output);
    WritePropertyValue("EnvironmentName", "EnvironmentName", logEvent.Properties, output);
    WritePropertyValue("ServiceContext", "SourceContext", logEvent.Properties, output);
    WritePropertyValue("ServiceInstance", "MachineName", logEvent.Properties, output);

    output.Write(",\"TransactionID\":");
    JsonValueFormatter.WriteQuotedJsonString(logEvent.TraceId?.ToString() ?? Guid.NewGuid().ToString(), output);

    if (logEvent.Properties.Count != 0)
    {
      WriteProperties(logEvent.Properties, output);
    }

    output.Write('}');
  }

  private static void WriteProperties(
      IReadOnlyDictionary<string, LogEventPropertyValue> properties,
      TextWriter output)
  {
    output.Write(",\"Properties\":{");

    var precedingDelimiter = string.Empty;

    foreach (var property in properties)
    {
      output.Write(precedingDelimiter);
      precedingDelimiter = ",";

      JsonValueFormatter.WriteQuotedJsonString(property.Key, output);
      output.Write(':');
      new JsonValueFormatter().Format(property.Value, output);
    }

    output.Write('}');
  }
  
  private static void WritePropertyValue(string jsonPropertyName, string propertyKey, IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output) {
    var propertyValue = properties[propertyKey];
    if (propertyValue != null) {
      output.Write($",\"{jsonPropertyName}\":");
      new JsonValueFormatter().Format(propertyValue, output);
    }
  }

  private static void LogNonFormattableEvent(LogEvent logEvent, Exception e)
  {
    SelfLog.WriteLine(
        "Event at {0} with message template {1} could not be formatted into JSON and will be dropped: {2}",
        logEvent.Timestamp.ToString("o"),
        logEvent.MessageTemplate.Text,
        e);
  }
}
