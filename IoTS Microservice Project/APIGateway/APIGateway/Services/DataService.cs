using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGateway.Contracts;
using APIGateway.DTOs;
using APIGateway.Entities;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using APIGateway.DTOs.ResourceDTOs;

namespace APIGateway.Services
{
    public class DataService : IDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        private string _APILocation = "/api/road_air_temp/";
        private string _newestEndpoint = "get-newest/";
        private string _stationEndpoint = "get-data-station/";
        private string _timeframeEndpoint = "get-data-timeframe/";
        private string _locationEndpoint = "get-data-location/";

        public DataService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<List<RoadAndAirTempData>> GetNewest()
        {

            HttpResponseMessage response = await _unitOfWork.HttpClient.GetAsync(_unitOfWork.DataLocation + _APILocation + _newestEndpoint);
            return await JsonSerializer.DeserializeAsync<List<RoadAndAirTempData>>(await response.Content.ReadAsStreamAsync());
        }

        public async Task<List<RoadAndAirTempData>> GetFilteredStation(string StationName, DTOs.TimeframeDTO timeframe)
        {
            HttpResponseMessage response;
            
            if (timeframe is null)
            {
                response = await _unitOfWork.HttpClient.GetAsync(_unitOfWork.DataLocation + _APILocation + _stationEndpoint  + StationName + "/false/");
            }
            else
            {
                DTOs.ResourceDTOs.TimeframeDTO timeframeDTO = new DTOs.ResourceDTOs.TimeframeDTO();
                timeframeDTO.StationName = StationName;
                timeframeDTO.ReferentTime = timeframe.Timestamp;
                timeframeDTO.TimeframeSeconds = timeframe.Seconds;

                response = await _unitOfWork.HttpClient.PostAsync(_unitOfWork.DataLocation + _APILocation + _timeframeEndpoint,
                                                                  new StringContent(JsonSerializer.Serialize(timeframeDTO), Encoding.UTF8, "application/json"));
            }

            return await JsonSerializer.DeserializeAsync<List<RoadAndAirTempData>>(await response.Content.ReadAsStreamAsync());
        }

        public async Task<List<RoadAndAirTempData>> GetFilteredLocation(double latitude, double longitude, double radius, bool newest)
        {
            LocationRadiusDTO locationInfo = new LocationRadiusDTO();
            locationInfo.CenterLatitude = latitude;
            locationInfo.CenterLongitude = longitude;
            locationInfo.RadiusMeters = radius;
            locationInfo.Newest = newest;

            HttpResponseMessage response = await _unitOfWork.HttpClient.PostAsync(_unitOfWork.DataLocation + _APILocation + _locationEndpoint,
                                                                                  new StringContent(JsonSerializer.Serialize(locationInfo), Encoding.UTF8, "application/json"));
            return await JsonSerializer.DeserializeAsync<List<RoadAndAirTempData>>(await response.Content.ReadAsStreamAsync());
        }
    }
}
