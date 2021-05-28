using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGateway.Entities;
using APIGateway.DTOs;

namespace APIGateway.Contracts
{
    public interface IDataService
    {
        Task<List<RoadAndAirTempData>> GetNewest();
        Task<List<RoadAndAirTempData>> GetFilteredStation(string StationName, TimeframeDTO timeframe);
        Task<List<RoadAndAirTempData>> GetFilteredLocation(double latitude, double longitude, double radius, bool newest);
    }
}
