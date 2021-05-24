using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMicroservice.Contracts;
using Geolocation;

namespace DataMicroservice.Services
{
    public class GeolocationService : IGeolocationService
    {
        private double _maxDistance = 5;

        public double MaxDistance
        {
            get
            {
                return _maxDistance;
            }
        }

        public double CalculateDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            return GeoCalculator.GetDistance(
                new Coordinate { Latitude = latitude1, Longitude = longitude1 },
                new Coordinate { Latitude = latitude2, Longitude = longitude2 },
                3, DistanceUnit.Meters);
        }
    }
}
