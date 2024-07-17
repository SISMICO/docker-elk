using Serilog;

namespace dotnet;

class Program
{
    static void Main(string[] args)
    {
        using var log = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .WriteTo.Console()
        .WriteTo.Http(requestUri: "http://localhost:8080", queueLimitBytes: null)
        .CreateLogger();
        Console.WriteLine("Testing ELK Log");
        
        for (int i = 0; i < 100; i++)
        {
            var randomNumber = new Random().Next(0, 1000);
            log.Information($"[Dotnet] Logging Random Number: {randomNumber}", randomNumber);
            Thread.Sleep(1000);
        }
        
        Console.WriteLine("Goodbye!");
    }
}
