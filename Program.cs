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

            // Register a Lazy<IMyService> that will be initialized later
            services.AddSingleton(provider =>
            {
                return new Lazy<IMyService>(() =>
                {
                    // This will be executed after 30 seconds delay
                    Console.WriteLine("IMyService initialized after delay.");
                    return new MyService();
                });
            });
        });

        var ihost = host.Build();

        _ = Task.Run(async () => {
            await ihost.RunAsync();

            var timer = new Timer((state) =>
            {
                var lazyService = ihost.Services.GetService<Lazy<IMyService>>();
                var myService = lazyService?.Value; // This triggers the initialization
                Console.WriteLine("IMyService is now available.");
            }, null, TimeSpan.FromSeconds(5), Timeout.InfiniteTimeSpan);

        });

        await ihost.RunAsync();
    }
}