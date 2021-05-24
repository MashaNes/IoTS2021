using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using RabbitMQ.Client;

namespace DataMicroservice.Contracts
{
    public interface IUnitOfWork
    {
        ISession CassandraSession { get; }
        IModel RabbitMQChannel { get; }
        string TemperatureTable { get; }
        string RabbitMQQueue { get; }
    }
}
