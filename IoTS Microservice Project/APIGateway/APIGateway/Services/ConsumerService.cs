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

namespace APIGateway.Services
{
    public class ConsumerService : IConsumerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConsumerService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public void Consume()
        {
            var Consumer = new EventingBasicConsumer(_unitOfWork.RabbitMQChannel);
            Consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                EventCommand data = JsonSerializer.Deserialize<EventCommand>(message);
                NotifyClients(data);
            };
            _unitOfWork.RabbitMQChannel.BasicConsume(queue: _unitOfWork.RabbitMQQueue,
                                                     autoAck: true,
                                                     consumer: Consumer);
        }

        private void NotifyClients(EventCommand data)
        {
            Console.WriteLine("Clients notified");
        }
    }
}
