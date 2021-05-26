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

        public NotifyService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task NotifyClient(TemperatureEvent Event)
        {
            string clientLoc = _unitOfWork.GetClientLocation(Event.StationName);
            if (clientLoc is not null)
            {
                if(Event.EventType == EventType.TempDropAlert || Event.EventType == EventType.TempRiseAlert)
                {
                    string loc = Event.DataInfluenced == DataInfluenced.RoadTemperature ? "road" : "air";
                    string type = Event.EventType == EventType.TempRiseAlert ? "sudden temperature rise" : "sudden temperature fall";
                    await sendCommand(clientLoc + _unitOfWork.DiagnosticsEndpoint + "?" + _unitOfWork.LocationParam + "='" + loc + "'&" + _unitOfWork.EventParam + "='" + type +"'");
                }
                else if(Event.DataInfluenced == DataInfluenced.AirTemperature)
                {
                    if (Event.EventType == EventType.TempNormalized)
                        await sendCommand(clientLoc + _unitOfWork.TempNormalizationEndpoint);
                    else if (Event.EventType == EventType.ColdAlert)
                        await sendCommand(clientLoc + _unitOfWork.HeatingEndpoint + "?" + _unitOfWork.TempParam + "=" + (20 - Event.Value));
                    else if (Event.EventType == EventType.HotAlert)
                        await sendCommand(clientLoc + _unitOfWork.CoolingEndpoint + "?" + _unitOfWork.TempParam + "=" + ((70 - Event.Value) / 2f));
                }
                else
                {
                    int psi = Event.EventType == EventType.ColdAlert ? 40 : Event.EventType == EventType.HotAlert ? 32 : 36;
                    await sendCommand(clientLoc + _unitOfWork.TirePressureEndpoint + "?" + _unitOfWork.PressureParam + "=" + psi);
                }
            }
        }

        private async Task sendCommand(string url)
        {
            HttpResponseMessage response = await _unitOfWork.HttpClient.PutAsync(url, null);
            await response.Content.ReadAsStringAsync();
        }
    }
}
