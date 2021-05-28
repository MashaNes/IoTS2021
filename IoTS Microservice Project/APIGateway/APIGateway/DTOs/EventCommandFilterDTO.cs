using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGateway.Entities;

namespace APIGateway.DTOs
{
    public class EventCommandFilterDTO
    {
        public TimeFromToDTO Timeframe { get; set; }
        public string StationName { get; set; }
        public EventType? EventType { get; set; }
        public DataInfluenced? DataInfluenced { get; set; }
    }
}
