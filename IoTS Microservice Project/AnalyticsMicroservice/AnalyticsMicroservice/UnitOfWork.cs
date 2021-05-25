using System;
using System.Collections.Generic;
using System.Text;
using AnalyticsMicroservice.Contracts;
using Cassandra;
using RabbitMQ.Client;

namespace AnalyticsMicroservice
{
    public class UnitOfWork : IUnitOfWork
    {
        private ISession _cassandraSession;
        public ISession CassandraSession
        {
            get
            {
                if (this._cassandraSession == null)
                {
                    Cluster cluster = Cluster.Builder().AddContactPoint("192.168.0.26").WithPort(9045).Build();
                    this._cassandraSession = cluster.Connect("event_data");
                }

                return _cassandraSession;
            }
        }

        private IModel _rabbitMQInputChannel;
        public IModel RabbitMQInputChannel
        {
            get
            {
                if (this._rabbitMQInputChannel == null)
                {
                    ConnectionFactory factory = new ConnectionFactory()
                    {
                        HostName = "rabbitmq",
                        Port = 5672,
                        UserName = "guest",
                        Password = "guest"
                    };

                    IConnection connection = factory.CreateConnection();
                    this._rabbitMQInputChannel = connection.CreateModel();
                    this._rabbitMQInputChannel.QueueDeclare(queue: this.RabbitMQInputQueue,
                                                            durable: false,
                                                            exclusive: false,
                                                            autoDelete: false,
                                                            arguments: null);
                }

                return this._rabbitMQInputChannel;
            }
        }

        private IModel _rabbitMQOutputChannel;
        public IModel RabbitMQOutputChannel
        {
            get
            {
                if (this._rabbitMQOutputChannel == null)
                {
                    ConnectionFactory factory = new ConnectionFactory()
                    {
                        HostName = "rabbitmq",
                        Port = 5672,
                        UserName = "guest",
                        Password = "guest"
                    };

                    IConnection connection = factory.CreateConnection();
                    this._rabbitMQOutputChannel = connection.CreateModel();
                    this._rabbitMQOutputChannel.QueueDeclare(queue: this.RabbitMQOutputQueue,
                                                             durable: false,
                                                             exclusive: false,
                                                             autoDelete: false,
                                                             arguments: null);
                }

                return this._rabbitMQOutputChannel;
            }
        }

        private string _eventTable = "events";
        public string EventTable
        {
            get
            {
                return this._eventTable;
            }
        }

        private string _rabbitMQInputQueue = "temp_data";
        public string RabbitMQInputQueue
        {
            get
            {
                return this._rabbitMQInputQueue;
            }
        }

        private string _rabbitMQOutputQueue = "event_data";
        public string RabbitMQOutputQueue
        {
            get
            {
                return this._rabbitMQOutputQueue;
            }
        }
    }
}
