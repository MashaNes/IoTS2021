using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGateway.Entities;
using APIGateway.DTOs;
using APIGateway.Contracts;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace APIGateway.Services
{
    public class CommandService : ICommandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private string _APILocation = "/api/event-command/";
        private string _filteredEndpoint = "get-filtered-data/";

        public CommandService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<List<EventCommand>> getFilteredData(EventCommandFilterDTO filterData)
        {
            HttpResponseMessage response = await _unitOfWork.HttpClient.PostAsync(_unitOfWork.CommandLocation + _APILocation + _filteredEndpoint,
                                                                                  new StringContent(JsonSerializer.Serialize(filterData), Encoding.UTF8, "application/json"));
            return await JsonSerializer.DeserializeAsync<List<EventCommand>>(await response.Content.ReadAsStreamAsync());
        }
    }
}
