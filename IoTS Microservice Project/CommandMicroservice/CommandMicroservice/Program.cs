using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommandMicroservice.Contracts;
using CommandMicroservice.Services;

namespace CommandMicroservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Thread.Sleep(TimeSpan.FromSeconds(60));
            IUnitOfWork unitOfWork = new UnitOfWork();
            ICassandraService cassandraService = new CassandraService(unitOfWork);
            IMessageService messageService = new MessageService(unitOfWork);
            INotifyService notify = new NotifyService(unitOfWork, messageService, cassandraService);
            IConsumerService consumer = new ConsumerService(unitOfWork, notify);
            consumer.Consume();
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
