using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandMicroservice.Contracts;
using CommandMicroservice.Entities;
using System.Text.Json;
using System.Text;

namespace CommandMicroservice.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MessageService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public void Enqueue(EventCommand data)
        {
            string body = JsonSerializer.Serialize(data);
            _unitOfWork.NotificationChannel.BasicPublish(exchange: "",
                                                         routingKey: _unitOfWork.NotificationQueue,
                                                         mandatory: true,
                                                         basicProperties: null,
                                                         body: Encoding.UTF8.GetBytes(body));
        }
    }
}
