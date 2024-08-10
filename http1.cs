using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;

namespace injectDelayFunc
{
    public class http1
    {
        private readonly ILogger<http1> _logger;
        private readonly IMyService _myService;
        private readonly IOtherService _otherService;

        public http1(ILogger<http1> logger, IMyService myService, IOtherService otherService)
        {
            _logger = logger;
            _myService = myService;
            _otherService = otherService;
        }

        [Function("http1")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            _myService.MyServiceMethod();

            _otherService.OtherServiceMethod();

            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
