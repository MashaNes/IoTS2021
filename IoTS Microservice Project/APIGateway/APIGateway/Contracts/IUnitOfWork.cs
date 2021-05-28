using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Net.Http;

namespace APIGateway.Contracts
{
    public interface IUnitOfWork
    {
        HttpClient HttpClient { get; }
        IModel RabbitMQChannel { get; }
        string RabbitMQQueue { get; }
        string CommandLocation { get; }
        string DataLocation { get; }
    }
}
