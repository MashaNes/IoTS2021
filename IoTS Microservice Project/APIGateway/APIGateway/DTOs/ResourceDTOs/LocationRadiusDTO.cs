﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.DTOs.ResourceDTOs
{
    public class LocationRadiusDTO
    {
        public double CenterLatitude { get; set; }
        public double CenterLongitude { get; set; }
        public double RadiusMeters { get; set; }
        public bool Newest { get; set; }
    }
}
