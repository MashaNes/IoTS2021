using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;

namespace DataMicroservice.Contracts
{
    public interface IUnitOfWork
    {
        ISession CassandraSession { get; }
        string TemperatureTable { get; }
        string AirQualityTable { get; }
    }
}
