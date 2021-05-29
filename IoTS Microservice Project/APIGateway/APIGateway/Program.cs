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
using APIGateway.Services.MessagingService;
using System.Threading;
using Microsoft.AspNetCore.SignalR;

namespace APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Thread.Sleep(TimeSpan.FromSeconds(60));
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
