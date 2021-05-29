using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using APIGateway.Entities;
using Microsoft.Extensions.Hosting;
using System.Threading;
using APIGateway.Contracts;

namespace APIGateway.Services.MessagingService
{
    public class MessageControllerService : BackgroundService
    {
        private readonly IConsumerService _consumerService;

        public MessageControllerService(IConsumerService consumerService)
        {
            _consumerService = consumerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumerService.Consume();

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("executing background loop");
                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }
        }
    }
}
