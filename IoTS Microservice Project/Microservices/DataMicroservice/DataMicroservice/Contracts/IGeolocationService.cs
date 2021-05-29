using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataMicroservice.Contracts
{
    public interface IGeolocationService
    {
        double MaxDistance { get; }
        double CalculateDistance(double latitude1, double longitude1, double latitude2, double longitude2);
    }
}
