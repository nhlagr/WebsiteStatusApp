using Serilog;
using Serilog.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting.WindowsServices; // Eklenen using direktifi

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.File(@"C:\temp\workerservice\logFile.txt")
            .CreateLogger();

        try
        {
            Log.Information("Starting up the service");
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "There was a problem starting the service");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseWindowsService() // Ensure that the application runs as a Windows Service
            .UseSerilog() // Ensure that Serilog is used
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<Worker>();
            });
}

public class Worker : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Ýþlem kodunuz burada
            await Task.Delay(1000, stoppingToken);
        }
    }
}
