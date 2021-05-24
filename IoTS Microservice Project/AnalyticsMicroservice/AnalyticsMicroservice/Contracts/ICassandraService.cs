using System;
using System.Collections.Generic;
using System.Text;
using AnalyticsMicroservice.Entities;

namespace AnalyticsMicroservice.Contracts
{
    public interface ICassandraService
    {
        void InsertData(TemperatureEvent temperatureEvent);
    }
}
