using System;
using System.Collections.Generic;
using System.Text;
using AnalyticsMicroservice.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using AnalyticsMicroservice.Entities;

namespace AnalyticsMicroservice.Services
{
    public class ConsumeService : IConsumeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAnalyticsService _analyticsService;
        private readonly ICassandraService _cassandraService;
        private readonly IMessageService _messageService;

        public ConsumeService(IUnitOfWork unitOfWork, IAnalyticsService analyticsService, ICassandraService cassandraService, IMessageService messageService)
        {
            this._unitOfWork = unitOfWork;
            this._analyticsService = analyticsService;
            this._cassandraService = cassandraService;
            this._messageService = messageService;
        }

        public void Consume()
        {
            var Consumer = new EventingBasicConsumer(_unitOfWork.RabbitMQInputChannel);
            Consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                RoadAndAirTempData data = (RoadAndAirTempData)JsonSerializer.Deserialize(message, typeof(RoadAndAirTempData));
                List<TemperatureEvent> events = _analyticsService.AnalyseNewData(data);
                foreach(TemperatureEvent Event in events)
                {
                    _cassandraService.InsertData(Event);
                    _messageService.Enqueue(Event);
                }
            };
            _unitOfWork.RabbitMQInputChannel.BasicConsume(queue: _unitOfWork.RabbitMQInputQueue,
                                                          autoAck: true,
                                                          consumer: Consumer);
        }
    }
}
