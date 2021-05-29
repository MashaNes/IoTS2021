using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandMicroservice.Entities;

namespace CommandMicroservice.Contracts
{
    public interface ICassandraService
    {
        void InsertData(EventCommand eventCommand);
        List<EventCommand> SelectTimeframeFilteredQuery(string table, DateTime low, DateTime high, string StationName, EventType? eventType, DataInfluenced? dataInfluenced);
        List<EventCommand> SelectFilteredQuery(string table, string StationName, EventType? eventType, DataInfluenced? dataInfluenced);
    }
}
