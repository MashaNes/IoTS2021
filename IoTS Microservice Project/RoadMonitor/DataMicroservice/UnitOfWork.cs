using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using DataMicroservice.Contracts;

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
    }
}
