using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMicroservice.Contracts;
using DataMicroservice.Entities;
using Cassandra;
using DataMicroservice.DTOs;
using Geolocation;

namespace DataMicroservice.Services
{
    public class TempService : ITempService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TempService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<bool> AddData(RoadAndAirTempData newData)
        {
            String command = "insert into temp_condition (\"Timestamp\", \"StationName\", \"AirTemperature\", \"Latitude\", \"Longitude\", \"RecordId\", \"RoadTemperature\")";
            command += "values ('" + ConvertDateToString(newData.Timestamp) + "', '" + newData.StationName + "', " + newData.AirTemperature + ", " + newData.Latitude + ", " +
                    +newData.Longitude + ", " + newData.RecordId + ", " + newData.RoadTemperature + ");";
            var response = _unitOfWork.CassandraSession.Execute(command);
            return true;
        }

        public async Task<List<RoadAndAirTempData>> GetAllData()
        {
            List<RoadAndAirTempData> retList = new List<RoadAndAirTempData>();

            var data = _unitOfWork.CassandraSession.Execute("select * from temp_condition");

            foreach(var instance in data)
            {
                retList.Add(ConvertCassandraRow(instance));
            }

            return retList;
        }

        public async Task<RoadAndAirTempData> GetDataByRecordId(int recordId)
        {
            Row result = _unitOfWork.CassandraSession.Execute("select * from temp_condition where \"RecordId\"=" + recordId + " ALLOW FILTERING").FirstOrDefault();

            return ConvertCassandraRow(result);
        }

        public async Task<List<RoadAndAirTempData>> GetDataByStationName(String StationName, bool Newest)
        {
            List<RoadAndAirTempData> retList = new List<RoadAndAirTempData>();

            var data = _unitOfWork.CassandraSession.Execute("select * from temp_condition where \"StationName\"='" + StationName + "' ALLOW FILTERING");

            RoadAndAirTempData newestInfo = null;

            foreach (var instance in data)
            {
                RoadAndAirTempData roadData = ConvertCassandraRow(instance);
                
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
            if (StationName == null)
                StationName = "";
            else
                StationName = "and \"StationName\"='" + StationName + "' ";

            DateTime low = time.AddSeconds(-seconds);
            DateTime high = time.AddSeconds(seconds);

            List<RoadAndAirTempData> retList = new List<RoadAndAirTempData>();

            var data = _unitOfWork.CassandraSession.Execute("select * from temp_condition where \"Timestamp\" > '" + ConvertDateToString(low)
                     + "' and \"Timestamp\" < '" + ConvertDateToString(high) + "' "+ StationName + "ALLOW FILTERING");

            foreach (var instance in data)
            {
                retList.Add(ConvertCassandraRow(instance));
            }

            return retList;
        }

        public async Task<List<RoadAndAirTempData>> GetDataLocation(LocationRadiusDTO locationInfo)
        {
            List<RoadAndAirTempData> retList = new List<RoadAndAirTempData>();

            Coordinate center = new Coordinate(locationInfo.CenterLatitude, locationInfo.CenterLongitude);

            var data = _unitOfWork.CassandraSession.Execute("select * from temp_condition");
            RoadAndAirTempData newest = null;

            foreach (var instance in data)
            {
                RoadAndAirTempData roadData = ConvertCassandraRow(instance);
                Coordinate point = new Coordinate(roadData.Latitude, roadData.Longitude);
                double distance = GeoCalculator.GetDistance(center, point, 3, DistanceUnit.Meters);
                if(distance <= locationInfo.RadiusMeters)
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

        private RoadAndAirTempData ConvertCassandraRow(Row instance)
        {
            RoadAndAirTempData roadInfo = new RoadAndAirTempData();
            roadInfo.Timestamp = DateTime.Parse(instance["Timestamp"].ToString());
            roadInfo.StationName = instance["StationName"].ToString();
            roadInfo.AirTemperature = (double)instance["AirTemperature"];
            roadInfo.Latitude = (double)instance["Latitude"];
            roadInfo.Longitude = (double)instance["Longitude"];
            roadInfo.RecordId = (int)instance["RecordId"];
            roadInfo.RoadTemperature = (double)instance["RoadTemperature"];
            return roadInfo;
        }

        private String TwoCharacterString(int number)
        {
            if (number < 10)
                return "0" + number;
            else return number.ToString();
        }

        private String ConvertDateToString(DateTime date)
        {
            String result = date.Year + "-" + TwoCharacterString(date.Month) + "-" + TwoCharacterString(date.Day) + "T"
                          + TwoCharacterString(date.Hour) + ":" + TwoCharacterString(date.Minute) + ":" + TwoCharacterString(date.Second);
            return result;
        }
    }
}
