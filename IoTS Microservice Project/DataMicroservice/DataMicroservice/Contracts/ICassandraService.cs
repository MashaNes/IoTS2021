using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMicroservice.Entities;
using Cassandra;

namespace DataMicroservice.Contracts
{
    public interface ICassandraService
    {
        RoadAndAirTempData ConvertCassandraTempRow(Row instance);
        string InsertRoadAndAirTempDataQuery(string table, RoadAndAirTempData data);
        string SelectAllQuery(string table);
        string SelectWhereQuery(string table, string parameter, string value);
        string SelectTimeframeQuery(string table, DateTime low, DateTime high);
        string SelectTimeframeStationQuery(string table, DateTime low, DateTime high, string station);
    }
}
