using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMicroservice.Contracts;
using DataMicroservice.Entities;

namespace DataMicroservice.Services
{
    public class TempService : ITempService
    {
        public async Task<bool> AddData(RoadAndAirTempData newData)
        {
            return true;
        }
    }
}
