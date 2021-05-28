﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Entities
{
    public class EventCommand
    {
        public TemperatureEvent TemperatureEvent { get; set; }
        public ActuationCommand ActuationCommand { get; set; }
    }
}
