using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Net.Http;
using Cassandra;

namespace CommandMicroservice.Contracts
{
    public interface IUnitOfWork
    {
        IModel RabbitMQChannel { get; }
        IModel NotificationChannel { get; }
        HttpClient HttpClient { get; }
        ISession CassandraSession { get; }
        string RabbitMQQueue { get; }
        string NotificationQueue { get; }
        bool AddClient(string station, string location);
        string GetClientLocation(string station);
        string CassandraTable { get; }
        string TempNormalizationEndpoint { get; }
        string HeatingEndpoint { get; }
        string CoolingEndpoint { get; }
        string DiagnosticsEndpoint { get; }
        string TirePressureEndpoint { get; }
        string TempParam { get; }
        string LocationParam { get; }
        string EventParam { get; }
        string PressureParam { get; }
    }
}
