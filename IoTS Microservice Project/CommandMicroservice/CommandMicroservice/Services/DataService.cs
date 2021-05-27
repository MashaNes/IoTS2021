using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandMicroservice.Contracts;
using CommandMicroservice.Entities;
using CommandMicroservice.DTOs;

namespace CommandMicroservice.Services
{
    public class DataService : IDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICassandraService _cassandraService;

        public DataService(IUnitOfWork unitOfWork, ICassandraService cassandraService)
        {
            this._unitOfWork = unitOfWork;
            this._cassandraService = cassandraService;
        }

        public List<EventCommand> getFilteredData(EventCommandFilterDTO eventCommandFilterDTO)
        {
            if (eventCommandFilterDTO.Timeframe == null)
                return _cassandraService.SelectFilteredQuery(_unitOfWork.CassandraTable, eventCommandFilterDTO.StationName, eventCommandFilterDTO.EventType, eventCommandFilterDTO.DataInfluenced);
            return _cassandraService.SelectTimeframeFilteredQuery(_unitOfWork.CassandraTable, eventCommandFilterDTO.Timeframe.From, eventCommandFilterDTO.Timeframe.To,
                eventCommandFilterDTO.StationName, eventCommandFilterDTO.EventType, eventCommandFilterDTO.DataInfluenced);
        }
    }
}
