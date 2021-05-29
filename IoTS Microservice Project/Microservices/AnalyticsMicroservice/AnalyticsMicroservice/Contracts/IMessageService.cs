using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AnalyticsMicroservice.Entities;

namespace AnalyticsMicroservice.Contracts
{
    public interface IMessageService
    {
        void EnqueueOutput(TemperatureEvent data);
        Task SendCEP(RoadAndAirTempData data);
    }
}
