using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using CommandMicroservice.Contracts;
using System.Text.Json;
using System.Text;
using CommandMicroservice.Entities;
using Microsoft.Extensions.Hosting;

namespace CommandMicroservice.Services
{
    public class ConsumerService : IConsumerService
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly INotifyService _notifyService;

        public ConsumerService(IUnitOfWork unitOfWork, INotifyService notifyService)
        {
            this._unitOfWork = unitOfWork;
            this._notifyService = notifyService;
        }

        public void Consume()
        {
            var Consumer = new EventingBasicConsumer(_unitOfWork.RabbitMQChannel);
            Consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                TemperatureEvent data = (TemperatureEvent)JsonSerializer.Deserialize(message, typeof(TemperatureEvent));
                _notifyService.NotifyClient(data);
            };
            _unitOfWork.RabbitMQChannel.BasicConsume(queue: _unitOfWork.RabbitMQQueue,
                                                     autoAck: true,
                                                     consumer: Consumer);
        }
    }
}
