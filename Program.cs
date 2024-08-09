using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWebApplication()
            .ConfigureServices(services =>
            {
                services.AddApplicationInsightsTelemetryWorkerService();
                services.ConfigureFunctionsApplicationInsights();

                // Register MyService as Singleton
                services.AddSingleton<IMyService, MyService>();

                // Register the custom hosted service that will add another service after a delay
                services.AddHostedService<DelayedServiceRegistrar>();
            })
            .Build();

        await host.RunAsync();
    }
}

public class DelayedServiceRegistrar : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceCollection _serviceCollection;

    public DelayedServiceRegistrar(IServiceProvider serviceProvider, IServiceCollection serviceCollection)
    {
        _serviceProvider = serviceProvider;
        _serviceCollection = serviceCollection;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Delay the injection by 5 seconds
        await Task.Delay(5000, cancellationToken);

        // Dynamically add another service to the service collection
        _serviceCollection.AddSingleton<IOtherService, OtherService>();

        // Trigger the rebuild of the service provider
        var serviceProvider = _serviceCollection.BuildServiceProvider();

        // Optionally resolve and initialize the newly added service
        var otherService = serviceProvider.GetService<IOtherService>();
        // Use otherService if needed
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

// Sample interface and implementation for the delayed service
public interface IOtherService
{
    void OtherServiceMethod();
}

public class OtherService : IOtherService
{
    public void OtherServiceMethod()
    {
        Console.WriteLine("OtherServiceMethod Executed!!!");
        // Implementation of the other service method
    }
}

// Sample interface and implementation for MyService
//public interface IMyService
//{
//    void MyServiceMethod();
//}

//public class MyService : IMyService
//{
//    public void MyServiceMethod()
//    {
//        // Implementation of your service method
//    }
//}