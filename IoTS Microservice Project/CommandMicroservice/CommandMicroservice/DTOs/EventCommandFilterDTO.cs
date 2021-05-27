using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandMicroservice.Entities;

namespace CommandMicroservice.DTOs
{
    public class EventCommandFilterDTO
    {
        public TimeframeDTO Timeframe { get; set; }
        public string StationName { get; set; }
        public EventType EventType { get; set; }
        public DataInfluenced DataInfluenced { get; set; }
    }
}
