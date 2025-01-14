﻿using injectDelayFunc;
using Microsoft.Extensions.DependencyInjection;
using System;

public class MyService : IMyService
{
    public IServiceCollection? _serviceCollection;
    public IServiceProvider? _serviceProvider;

    public MyService()
    {
        this.CreateServiceCollection();
        // Use AddHttpClient here or in the MyService class
    }

    public void CreateServiceCollection()
    {
        _serviceCollection = new ServiceCollection();

        _serviceCollection.AddSingleton<IMyService2, MyService2>();

        _serviceProvider = _serviceCollection.BuildServiceProvider();
    }

    public void MyServiceMethod()
    {
        Console.WriteLine("MyServiceMethod Executed");
    }
}