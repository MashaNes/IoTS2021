using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandMicroservice.Entities;
using CommandMicroservice.DTOs;

namespace CommandMicroservice.Contracts
{
    public interface IDataService
    {
        List<EventCommand> getFilteredData(EventCommandFilterDTO eventCommandFilterDTO);
    }
}
