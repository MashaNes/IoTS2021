using System.Text;
using AnalyticsMicroservice.Contracts;
using System.Text.Json;
using AnalyticsMicroservice.Entities;

namespace AnalyticsMicroservice.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MessageService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public void EnqueueOutput(TemperatureEvent data)
        {
            string body = JsonSerializer.Serialize(data);
            _unitOfWork.RabbitMQOutputChannel.BasicPublish(exchange: "",
                                                           routingKey: _unitOfWork.RabbitMQOutputQueue,
                                                           mandatory: true,
                                                           basicProperties: null,
                                                           body: Encoding.UTF8.GetBytes(body));
        }

        public void EnqueueCEP(RoadAndAirTempData data)
        {
            string body = JsonSerializer.Serialize(data);
            _unitOfWork.RabbitMQCEPChannel.BasicPublish(exchange: "",
                                                        routingKey: _unitOfWork.RabbitMQCEPQueue,
                                                        mandatory: true,
                                                        basicProperties: null,
                                                        body: Encoding.UTF8.GetBytes(body));
        }
    }
}
