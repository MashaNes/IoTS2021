using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using APIGateway.Contracts;
using RabbitMQ.Client;

namespace APIGateway
{
    public class UnitOfWork : IUnitOfWork
    {
        private HttpClient _httpClient;
        public HttpClient HttpClient
        {
            get
            {
                if (this._httpClient is null)
                {
                    this._httpClient = new HttpClient();
                    this._httpClient.DefaultRequestHeaders.Accept.Clear();
                    this._httpClient.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                }

                return this._httpClient;
            }
        }

        private IModel _rabbitMQChannel;
        public IModel RabbitMQChannel
        {
            get
            {
                if (this._rabbitMQChannel == null)
                {
                    ConnectionFactory factory = new ConnectionFactory()
                    {
                        HostName = "192.168.0.26",
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

        private string _rabbitMQQueue = "notifications";
        public string RabbitMQQueue
        {
            get
            {
                return this._rabbitMQQueue;
            }
        }

        private string _commandHost = "http://192.168.0.26";
        private int _commandPort = 49155;
        public string CommandLocation
        {
            get
            {
                return _commandHost + ":" + _commandPort;
            }
        }

        private string _dataHost = "http://192.168.0.26";
        private int _dataPort = 49164;
        public string DataLocation
        {
            get
            {
                return _dataHost + ":" + _dataPort;
            }
        }
    }
}
