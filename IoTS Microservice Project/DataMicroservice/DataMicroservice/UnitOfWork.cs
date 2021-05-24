using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using DataMicroservice.Contracts;
using RabbitMQ.Client;

namespace DataMicroservice
{
    public class UnitOfWork : IUnitOfWork
    {
        private ISession _cassandraSession;
        public ISession CassandraSession
        {
            get
            {
                if(this._cassandraSession == null)
                {
                    Cluster cluster = Cluster.Builder().AddContactPoint("192.168.0.26").Build();
                    this._cassandraSession = cluster.Connect("road_data");
                }

                return _cassandraSession;
            }
        }

        private IModel _rabbitMQChannel;
        public IModel RabbitMQChannel
        {
            get
            {
                if(this._rabbitMQChannel == null)
                {
                    ConnectionFactory factory = new ConnectionFactory()
                    {
                        HostName = "rabbitmq",
                        Port = 5672,
                        UserName = "guest",
                        Password = "guest"
                    };

                    IConnection connection = factory.CreateConnection();
                    this._rabbitMQChannel = connection.CreateModel();
                    this._rabbitMQChannel.QueueDeclare(queue: this.RabbitMQQueue,
                                                       durable: false,
                                                       exclusive: false,
                                                       autoDelete: false,
                                                       arguments: null);
                }

                return this._rabbitMQChannel;
            }
        }

        private string _temperatureTable = "temp_condition";
        public string TemperatureTable
        {
            get
            {
                return this._temperatureTable;
            }
        }

        private string _airQualityTable = "air_condition";
        public string AirQualityTable
        {
            get
            {
                return this._airQualityTable;
            }
        }

        private string _rabbitMQQueue = "temp_data";
        public string RabbitMQQueue
        {
            get
            {
                return this._rabbitMQQueue;
            }
        }
    }
}
