using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMicroservice.Contracts;
using DataMicroservice.Entities;
using System.Text.Json;
using System.Text;

namespace DataMicroservice.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MessageService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public void Enqueue(RoadAndAirTempData data)
        {
            string body = JsonSerializer.Serialize(data);
            _unitOfWork.RabbitMQChannel.BasicPublish(exchange: "",
                                                     routingKey: _unitOfWork.RabbitMQQueue,
                                                     mandatory: true,
                                                     basicProperties: null,
                                                     body: Encoding.UTF8.GetBytes(body));
        }
    }
}
