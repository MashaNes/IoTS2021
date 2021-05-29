using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandMicroservice.DTOs
{
    public class EventDTO
    {
        public int DataInfluenced { get; set; }
        public int EventType { get; set; }
        public double Value { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Timestamp { get; set; }
        public string StationName { get; set; }
    }
}
