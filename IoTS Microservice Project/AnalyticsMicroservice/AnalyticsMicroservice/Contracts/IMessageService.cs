using System;
using System.Collections.Generic;
using System.Text;
using AnalyticsMicroservice.Entities;

namespace AnalyticsMicroservice.Contracts
{
    public interface IMessageService
    {
        void EnqueueOutput(TemperatureEvent data);
        void EnqueueCEP(RoadAndAirTempData data);
    }
}
