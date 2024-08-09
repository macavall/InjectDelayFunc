using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWebApplication()
            .ConfigureServices(services =>
            {
                services.AddApplicationInsightsTelemetryWorkerService();
                services.ConfigureFunctionsApplicationInsights();

                // Register a Lazy<IMyService> for delayed initialization
                services.AddSingleton(provider =>
                {
                    return new Lazy<IMyService>(() =>
                    {
                        return new MyService();
                    });
                });
            })
            .Build();

        // Run the host
        var runTask = Task.Run(() => host.Run());

        // Delay for 5 seconds
        await Task.Delay(5000);

        // Trigger the service initialization
        var lazyService = host.Services.GetRequiredService<Lazy<IMyService>>();
        var myService = lazyService.Value; // This triggers the initialization

        // Wait for the host to complete 
        await runTask;
    }
}