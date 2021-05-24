using System;
using System.Collections.Generic;
using System.Text;
using Cassandra;
using RabbitMQ.Client;

namespace AnalyticsMicroservice.Contracts
{
    public interface IUnitOfWork
    {
        ISession CassandraSession { get; }
        IModel RabbitMQInputChannel { get; }
        IModel RabbitMQOutputChannel { get; }
        string EventTable { get; }
        string RabbitMQInputQueue { get; }
        string RabbitMQOutputQueue { get; }
    }
}
