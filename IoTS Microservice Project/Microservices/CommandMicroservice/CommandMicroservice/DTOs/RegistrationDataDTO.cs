using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandMicroservice.DTOs
{
    public class RegistrationDataDTO
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string StationName { get; set; }
    }
}
