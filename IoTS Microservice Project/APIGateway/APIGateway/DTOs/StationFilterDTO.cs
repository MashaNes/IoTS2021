using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.DTOs
{
    public class StationFilterDTO
    {
        public string StationName { get; set; }
        public TimeframeDTO Timeframe { get; set; }
    }
}
