using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMicroservice.Contracts;
using DataMicroservice.Entities;
using Cassandra;
using DataMicroservice.DTOs;

namespace DataMicroservice.Services
{
    public class TempService : ITempService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICassandraService _cassandraService;
        private readonly IGeolocationService _geolocationService;
        private readonly IMessageService _messageService;

        public TempService(IUnitOfWork unitOfWork, ICassandraService cassandraService, IGeolocationService geolocationService, IMessageService messageService)
        {
            this._unitOfWork = unitOfWork;
            this._cassandraService = cassandraService;
            this._geolocationService = geolocationService;
            this._messageService = messageService;
        }

        public async Task<bool> AddData(RoadAndAirTempData newData)
        {
            newData.RoadTemperature = convertTempFtoC(newData.RoadTemperature);
            newData.AirTemperature = convertTempFtoC(newData.AirTemperature);
            _cassandraService.InsertRoadAndAirTempDataQuery(_unitOfWork.TemperatureTable, newData);
            _messageService.Enqueue(newData);
            return true;
        }

        private double convertTempFtoC(double tempInF)
        {
            return (tempInF - 32) / (double)1.8;
        }

        public async Task<List<RoadAndAirTempData>> GetAllData()
        {
            var data = _unitOfWork.CassandraSession.Execute(_cassandraService.SelectAllQuery(_unitOfWork.TemperatureTable));
            return data.Select(instance => _cassandraService.ConvertCassandraTempRow(instance)).ToList();
        }

        public async Task<List<RoadAndAirTempData>> GetNewest()
        {
            var data = _unitOfWork.CassandraSession.Execute(_cassandraService.SelectAllQuery(_unitOfWork.TemperatureTable));
            Dictionary<string, RoadAndAirTempData> newest = new Dictionary<string, RoadAndAirTempData>();
            foreach(var instance in data)
            {
                RoadAndAirTempData roadData = _cassandraService.ConvertCassandraTempRow(instance);
                if (!newest.ContainsKey(roadData.StationName))
                    newest.Add(roadData.StationName, roadData);
                else if (newest[roadData.StationName].Timestamp < roadData.Timestamp)
                    newest[roadData.StationName] = roadData;
            }
            return newest.Values.ToList();
        }

        public async Task<RoadAndAirTempData> GetDataByRecordId(int recordId)
        {
            Row result = _unitOfWork.CassandraSession.Execute(_cassandraService.SelectWhereQuery(_unitOfWork.TemperatureTable, "RecordId", recordId.ToString())).FirstOrDefault();
            return _cassandraService.ConvertCassandraTempRow(result);
        }

        public async Task<List<RoadAndAirTempData>> GetDataByStationName(String StationName, bool Newest)
        {
            List<RoadAndAirTempData> retList = new List<RoadAndAirTempData>();

            var data = _unitOfWork.CassandraSession.Execute(_cassandraService.SelectWhereQuery(_unitOfWork.TemperatureTable, "StationName", "'" + StationName + "'"));

            RoadAndAirTempData newestInfo = null;

            foreach (var instance in data)
            {
                RoadAndAirTempData roadData = _cassandraService.ConvertCassandraTempRow(instance);
                
                if(!Newest)
                    retList.Add(roadData);
                else if (newestInfo == null || roadData.Timestamp > newestInfo.Timestamp)
                    newestInfo = roadData;
            }

            if (Newest)
                retList.Add(newestInfo);

            return retList;
        }

        public async Task<List<RoadAndAirTempData>> GetDataByTimestamp(String StationName, DateTime time, int seconds)
        {
            DateTime low = time.AddSeconds(-seconds);
            DateTime high = time.AddSeconds(seconds);

            RowSet data;
            if (StationName is null)
                data = _unitOfWork.CassandraSession.Execute(_cassandraService.SelectTimeframeQuery(_unitOfWork.TemperatureTable, low, high));
            else
                data = _unitOfWork.CassandraSession.Execute(_cassandraService.SelectTimeframeStationQuery(_unitOfWork.TemperatureTable, low, high, StationName));

            return data.Select(instance => _cassandraService.ConvertCassandraTempRow(instance)).ToList();
        }

        public async Task<List<RoadAndAirTempData>> GetDataLocation(LocationRadiusDTO locationInfo)
        {
            List<RoadAndAirTempData> retList = new List<RoadAndAirTempData>();

            var data = _unitOfWork.CassandraSession.Execute(_cassandraService.SelectAllQuery(_unitOfWork.TemperatureTable));
            RoadAndAirTempData newest = null;

            foreach (var instance in data)
            {
                RoadAndAirTempData roadData = _cassandraService.ConvertCassandraTempRow(instance);
                if(_geolocationService.CalculateDistance(locationInfo.CenterLatitude, locationInfo.CenterLongitude, roadData.Latitude, roadData.Longitude) <= locationInfo.RadiusMeters)
                {
                    if (!locationInfo.Newest)
                        retList.Add(roadData);
                    else if (newest == null || newest.Timestamp < roadData.Timestamp)
                        newest = roadData;
                }
            }

            if (locationInfo.Newest)
                retList.Add(newest);

            return retList;
        }
    }
}
