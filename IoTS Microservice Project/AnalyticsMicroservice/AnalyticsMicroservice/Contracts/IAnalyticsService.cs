using System;
using System.Collections.Generic;
using System.Text;
using AnalyticsMicroservice.Entities;

namespace AnalyticsMicroservice.Contracts
{
    public interface IAnalyticsService
    {
        List<TemperatureEvent> AnalyseNewData(RoadAndAirTempData data);
    }
}
