using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandMicroservice.Contracts;
using CommandMicroservice.Entities;
using System.Net.Http;

namespace CommandMicroservice.Services
{
    public class NotifyService : INotifyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;
        private readonly ICassandraService _cassandraService;

        public NotifyService(IUnitOfWork unitOfWork, IMessageService messageService, ICassandraService cassandraService)
        {
            this._unitOfWork = unitOfWork;
            this._messageService = messageService;
            this._cassandraService = cassandraService;
        }

        public async Task NotifyClient(TemperatureEvent Event)
        {
            string command = "";
            EventCommand eventCommand = new EventCommand();
            eventCommand.TemperatureEvent = Event;
            eventCommand.ActuationCommand = new ActuationCommand();

            if(Event.EventType == EventType.TempDropAlert || Event.EventType == EventType.TempRiseAlert)
            {
                string loc = _unitOfWork.LocationParam + "='" + (Event.DataInfluenced == DataInfluenced.RoadTemperature ? "road" : "air") + "'";
                string type = _unitOfWork.EventParam + "='" + (Event.EventType == EventType.TempRiseAlert ? "sudden temperature rise" : "sudden temperature fall") + "'";
                command = _unitOfWork.DiagnosticsEndpoint + "?" + loc + "&" + type;
                eventCommand.ActuationCommand.Command = _unitOfWork.DiagnosticsEndpoint;
                eventCommand.ActuationCommand.AdditionalArguments.Add(loc);
                eventCommand.ActuationCommand.AdditionalArguments.Add(type);
            }
            else if(Event.DataInfluenced == DataInfluenced.AirTemperature)
            {
                if (Event.EventType == EventType.TempNormalized)
                {
                    command = _unitOfWork.TempNormalizationEndpoint;
                    eventCommand.ActuationCommand.Command = _unitOfWork.TempNormalizationEndpoint;
                }
                else if (Event.EventType == EventType.ColdAlert)
                {
                    string temp = _unitOfWork.TempParam + "=" + (20 - Event.Value);
                    command = _unitOfWork.HeatingEndpoint + "?" + temp;
                    eventCommand.ActuationCommand.Command = _unitOfWork.HeatingEndpoint;
                    eventCommand.ActuationCommand.AdditionalArguments.Add(temp);
                }
                else if (Event.EventType == EventType.HotAlert)
                {
                    string temp = _unitOfWork.TempParam + "=" + ((70 - Event.Value) / 2f);
                    command = _unitOfWork.CoolingEndpoint + "?" + temp;
                    eventCommand.ActuationCommand.Command = _unitOfWork.CoolingEndpoint;
                    eventCommand.ActuationCommand.AdditionalArguments.Add(temp);
                }
            }
            else
            {
                string psi = _unitOfWork.PressureParam + "=" + (Event.EventType == EventType.ColdAlert ? 40 : Event.EventType == EventType.HotAlert ? 32 : 36);
                command = _unitOfWork.TirePressureEndpoint + "?" + psi;
                eventCommand.ActuationCommand.Command = _unitOfWork.TirePressureEndpoint;
                eventCommand.ActuationCommand.AdditionalArguments.Add(psi);
            }

            string clientLoc = _unitOfWork.GetClientLocation(Event.StationName);
            if (clientLoc is not null)
                await sendCommand(clientLoc + command);
            _cassandraService.InsertData(eventCommand);
            _messageService.Enqueue(eventCommand);
        }

        private async Task sendCommand(string url)
        {
            HttpResponseMessage response = await _unitOfWork.HttpClient.PutAsync(url, null);
            await response.Content.ReadAsStringAsync();
        }
    }
}
