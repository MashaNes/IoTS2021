using System.Text;
using AnalyticsMicroservice.Contracts;
using System.Text.Json;
using AnalyticsMicroservice.Entities;
using AnalyticsMicroservice.DTOs;
using System.Threading.Tasks;
using System.Net.Http;

namespace AnalyticsMicroservice.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private string _sidhhiEndpoint = "/productionStream";

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

        public async Task SendCEP(RoadAndAirTempData data)
        {
            SidhhiDTO dto = new SidhhiDTO();
            dto.Event = new RoadAndAirTempDTO(data);

            HttpResponseMessage response = await _unitOfWork.HttpClient.PostAsync(_unitOfWork.SidhhiLocation + _sidhhiEndpoint,
                                                                                  new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"));
            await response.Content.ReadAsStreamAsync();
        }
    }
}
