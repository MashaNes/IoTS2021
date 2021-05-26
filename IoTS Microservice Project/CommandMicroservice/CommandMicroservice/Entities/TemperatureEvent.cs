using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandMicroservice.Entities
{
    public enum DataInfluenced
    {
        RoadTemperature,
        AirTemperature
    }

    public enum EventType
    {
        HotAlert,
        ColdAlert,
        TempNormalized,
        TempDropAlert,
        TempRiseAlert
    }

    public class TemperatureEvent
    {
        public DataInfluenced DataInfluenced { get; set; }
        public EventType EventType { get; set; }
        public double Value { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }
        public string StationName { get; set; }
    }
}
