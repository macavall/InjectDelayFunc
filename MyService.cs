using injectDelayFunc;
using Microsoft.Extensions.DependencyInjection;
using System;

public class MyService : IMyService
{
    public IServiceCollection? _serviceCollection;
    //public IServiceCollection? _serviceColl;
    public IServiceProvider? _serviceProvider;

    public MyService(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
        this.CreateServiceCollection();
        // Use AddHttpClient here or in the MyService class
    }

    public void CreateServiceCollection()
    {
        _serviceCollection?.AddSingleton<IOtherService, OtherService>();

        _serviceProvider = _serviceCollection?.BuildServiceProvider();

        _serviceProvider?.GetRequiredService<IOtherService>().OtherServiceMethod();

        var newServiceCollection = new ServiceCollection();
        newServiceCollection.AddSingleton<IMyService2, MyService2>();

        _serviceProvider = newServiceCollection.BuildServiceProvider();
    }

    public void MyServiceMethod()
    {
        Console.WriteLine("MyServiceMethod Executed");
    }
}