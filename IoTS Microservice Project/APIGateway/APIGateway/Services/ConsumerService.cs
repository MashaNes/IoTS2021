using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGateway.Contracts;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using APIGateway.Entities;
using System.Text.Json;
using APIGateway.Services.MessagingService;
using Microsoft.AspNetCore.SignalR;

namespace APIGateway.Services
{
    public class ConsumerService : IConsumerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<MessageHub> _hubContext;

        public ConsumerService(IUnitOfWork unitOfWork, IHubContext<MessageHub> hubContext)
        {
            this._unitOfWork = unitOfWork;
            this._hubContext = hubContext;
        }

        public void Consume()
        {
            var Consumer = new EventingBasicConsumer(_unitOfWork.RabbitMQChannel);
            Consumer.Received += async (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                EventCommand data = JsonSerializer.Deserialize<EventCommand>(message);
                await Notify(data);
            };
            _unitOfWork.RabbitMQChannel.BasicConsume(queue: _unitOfWork.RabbitMQQueue,
                                                     autoAck: true,
                                                     consumer: Consumer);
        }

        public async Task Notify(EventCommand data)
        {
            Console.WriteLine("Notify clients");
            await _hubContext.Clients.Group("events_commands").SendAsync("new_data", data);
        }
    }
}
