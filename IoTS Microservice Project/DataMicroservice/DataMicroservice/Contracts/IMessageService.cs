using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMicroservice.Entities;

namespace DataMicroservice.Contracts
{
    public interface IMessageService
    {
        void Enqueue(RoadAndAirTempData data);
    }
}
