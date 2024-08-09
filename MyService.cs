﻿using injectDelayFunc;
using Microsoft.Extensions.DependencyInjection;
using System;
using static System.Formats.Asn1.AsnWriter;

public class MyService : IMyService
{
    public IServiceCollection? _serviceCollection;
    public IServiceCollection? _serviceColl;
    public IServiceProvider? _serviceProvider;

    public MyService(IServiceCollection servColl)
    {
        _serviceColl = servColl;
        this.CreateServiceCollection();
        // Use AddHttpClient here or in the MyService class
    }

    public void CreateServiceCollection()
    {
        _serviceColl?.AddSingleton(new TestClass());

        _serviceCollection = new ServiceCollection();

        _serviceCollection.AddSingleton<IMyService2, MyService2>();

        _serviceProvider = _serviceCollection.BuildServiceProvider();
    }

    public void MyServiceMethod()
    {
        Console.WriteLine("MyServiceMethod Executed!!!");

        var myService2 = _serviceProvider?.GetRequiredService<IMyService2>();

        myService2?.MyServiceMethod2();


    }

    public class TestClass
    {
        public void TestMethod()
        {
            Console.WriteLine("TestMethod Executed!!!");
        }
    }
}