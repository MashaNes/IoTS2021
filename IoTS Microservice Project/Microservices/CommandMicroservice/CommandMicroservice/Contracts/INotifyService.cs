using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandMicroservice.Entities;

namespace CommandMicroservice.Contracts
{
    public interface INotifyService
    {
        Task NotifyClient(TemperatureEvent Event);
    }
}
