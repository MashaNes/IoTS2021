using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMicroservice.Contracts;
using Cassandra;
using DataMicroservice.Entities;

namespace DataMicroservice.Services
{
    public class CassandraService : ICassandraService
    {
        private readonly IDateService _dateService;

        public CassandraService(IDateService dateService)
        {
            this._dateService = dateService;
        }

        public RoadAndAirTempData ConvertCassandraTempRow(Row instance)
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

        public string InsertRoadAndAirTempDataQuery(string table, RoadAndAirTempData data)
        {
            string command = "insert into " + table + " (\"Timestamp\", \"StationName\", \"AirTemperature\", \"Latitude\", \"Longitude\", \"RecordId\", \"RoadTemperature\")";
            command += "values ('" + _dateService.ConvertDateToString(data.Timestamp) + "', '" + data.StationName + "', " + data.AirTemperature + ", " + data.Latitude + ", " +
                    +data.Longitude + ", " + data.RecordId + ", " + data.RoadTemperature + ");";
            return command;
        }

        public string SelectAllQuery(string table)
        {
            return "select * from " + table + ";";
        }

        public string SelectWhereQuery(string table, string parameter, string value)
        {
            return "select * from " + table + " where \"" + parameter + "\"=" + value + " ALLOW FILTERING";
        }

        public string SelectTimeframeQuery(string table, DateTime low, DateTime high)
        {
            string command = "select * from " + table + " where \"Timestamp\" > '" + _dateService.ConvertDateToString(low)
                           + "' and \"Timestamp\" < '" + _dateService.ConvertDateToString(high) + "' ALLOW FILTERING";
            return command;
        }

        public string SelectTimeframeStationQuery(string table, DateTime low, DateTime high, string station)
        {
            string command = "select * from " + table + " where \"Timestamp\" > '" + _dateService.ConvertDateToString(low)
                           + "' and \"Timestamp\" < '" + _dateService.ConvertDateToString(high) + "' and \"StationName\"='" + station + "' ALLOW FILTERING";
            return command;
        }
    }
}
