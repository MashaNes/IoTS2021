using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandMicroservice.DTOs;

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

        public TemperatureEvent()
        { }

        public TemperatureEvent(EventDTO dto)
        {
            DataInfluenced = (DataInfluenced)dto.DataInfluenced;
            EventType = (EventType)dto.EventType;
            Value = dto.Value;
            Latitude = dto.Latitude;
            Longitude = dto.Longitude;
            StationName = dto.StationName;
            Timestamp = DateTime.Parse(dto.Timestamp);
        }
    }
}
