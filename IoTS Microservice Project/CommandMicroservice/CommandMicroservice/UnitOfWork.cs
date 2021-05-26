using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandMicroservice.Contracts;
using RabbitMQ.Client;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.IO;

namespace CommandMicroservice
{
    public class UnitOfWork : IUnitOfWork
    {
        private IModel _rabbitMQChannel;
        public IModel RabbitMQChannel
        {
            get
            {
                if (this._rabbitMQChannel == null)
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

        private HttpClient _httpClient;
        public HttpClient HttpClient
        {
            get
            {
                if(this._httpClient is null)
                {
                    this._httpClient = new HttpClient();
                    this._httpClient.DefaultRequestHeaders.Accept.Clear();
                    this._httpClient.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                }

                return this._httpClient;
            }
        }

        private string _rabbitMQQueue = "event_data";
        public string RabbitMQQueue
        {
            get
            {
                return this._rabbitMQQueue;
            }
        }

        private string _clientFile = "clients.json";

        private Dictionary<string, string> _clients = new Dictionary<string, string>();
        public bool AddClient(string station, string location)
        {
            if (this._clients.ContainsKey(station))
                return false;
            this._clients.Add(station, location);

            if (File.Exists(_clientFile))
            {
                Dictionary<string, string> fileDict = (Dictionary<string, string>)JsonSerializer.Deserialize(File.ReadAllText(_clientFile), typeof(Dictionary<string, string>));
                foreach (string key in fileDict.Keys)
                {
                    if (!this._clients.ContainsKey(key))
                        this._clients.Add(key, fileDict[key]);
                }
            }
            File.WriteAllText(_clientFile, JsonSerializer.Serialize(_clients));

            return true;
        }
        public string GetClientLocation(string station)
        {
            if (this._clients.ContainsKey(station))
                return this._clients[station];
            if(File.Exists(_clientFile))
            {
                this._clients = (Dictionary<string, string>)JsonSerializer.Deserialize(File.ReadAllText(_clientFile), typeof(Dictionary<string, string>));
                if (this._clients.ContainsKey(station))
                    return this._clients[station];
                return null;
            }
            return null;
        }

        private string _tempNormalizationEndpoint = "normalized-temperature";
        public string TempNormalizationEndpoint
        {
            get
            {
                return this._tempNormalizationEndpoint;
            }
        }

        private string _heatingEndpoint = "start-heating";
        public string HeatingEndpoint
        {
            get
            {
                return this._heatingEndpoint;
            }
        }

        private string _coolingEndpoint = "start-cooling";
        public string CoolingEndpoint
        {
            get
            {
                return this._coolingEndpoint;
            }
        }

        private string _diagnosticsEndpoint = "start-diagnostics";
        public string DiagnosticsEndpoint
        {
            get
            {
                return this._diagnosticsEndpoint;
            }
        }

        private string _tirePressureEndpoint = "adjust-tire-pressure";
        public string TirePressureEndpoint
        {
            get
            {
                return this._tirePressureEndpoint;
            }
        }

        private string _tempParam = "temp";
        public string TempParam
        {
            get
            {
                return this._tempParam;
            }
        }

        private string _locationParam = "loc";
        public string LocationParam
        {
            get
            {
                return this._locationParam;
            }
        }

        private string _eventParam = "event";
        public string EventParam
        {
            get
            {
                return this._eventParam;
            }
        }

        private string _pressureParam = "psi";
        public string PressureParam
        {
            get
            {
                return this._pressureParam;
            }
        }
    }
}
