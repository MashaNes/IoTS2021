using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMicroservice.Entities;
using DataMicroservice.DTOs;

namespace DataMicroservice.Contracts
{
    public interface ITempService
    {
        Task<bool> AddData(RoadAndAirTempData newData);
        Task<List<RoadAndAirTempData>> GetAllData();
        Task<RoadAndAirTempData> GetDataByRecordId(int recordId);
        Task<List<RoadAndAirTempData>> GetDataByStationName(String StationName, bool Newest);
        Task<List<RoadAndAirTempData>> GetDataByTimestamp(String StationName, DateTime time, int seconds);
        Task<List<RoadAndAirTempData>> GetDataLocation(LocationRadiusDTO locationInfo);
    }
}
