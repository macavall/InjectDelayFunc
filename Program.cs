using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var host = new HostBuilder()
        .ConfigureFunctionsWebApplication()
        .ConfigureServices(async services =>
        {
            services.AddApplicationInsightsTelemetryWorkerService();
            services.ConfigureFunctionsApplicationInsights();

            _ = Task.Run(async () =>
            {
                await Task.Delay(5 * 1000);
                services.AddSingleton<IMyService, MyService>();
            });
        });

        var ihost = host.Build();

        ihost.Run();
    }
}