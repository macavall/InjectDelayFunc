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
                services.AddSingleton<DelayedServiceInitializer>();
            })
            .Build();

        // Start the host
        var runHostTask = host.RunAsync();

        // Delay the registration of IMyService for 5 seconds
        var delayedInitializer = host.Services.GetRequiredService<DelayedServiceInitializer>();
        await delayedInitializer.InitializeAsync();

        // Wait for the host to complete
        await runHostTask;
    }
}

// Utility class to handle delayed service registration
public class DelayedServiceInitializer
{
    private readonly IServiceProvider _serviceProvider;

    public DelayedServiceInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task InitializeAsync()
    {
        await Task.Delay(5000); // Delay for 5 seconds

        var services = _serviceProvider.GetRequiredService<IServiceCollection>();
        services.AddSingleton<IMyService, MyService>();
    }
}