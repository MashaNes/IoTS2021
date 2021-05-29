using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataMicroservice.DTOs
{
    public class TimeframeDTO
    {
        public int TimeframeSeconds { get; set; }
        public String StationName { get; set; }
        public DateTime ReferentTime { get; set; }
    }
}
