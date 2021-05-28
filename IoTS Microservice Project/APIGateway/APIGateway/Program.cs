using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGateway.Contracts;
using APIGateway.Services;
using System.Threading;

namespace APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Thread.Sleep(TimeSpan.FromSeconds(40));
            IUnitOfWork unitOfWork = new UnitOfWork();
            IConsumerService consumerService = new ConsumerService(unitOfWork);
            consumerService.Consume();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
