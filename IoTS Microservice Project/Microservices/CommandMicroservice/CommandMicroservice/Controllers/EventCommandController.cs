using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandMicroservice.Contracts;
using CommandMicroservice.DTOs;

namespace CommandMicroservice.Controllers
{
    [ApiController]
    [Route("api/event-command")]
    public class EventCommandController : ControllerBase
    {
        private readonly IDataService _dataService;

        public EventCommandController(IDataService dataService)
        {
            this._dataService = dataService;
        }

        [HttpPost]
        [Route("get-filtered-data")]
        public async Task<IActionResult> GetFilteredData([FromBody] EventCommandFilterDTO filterInfo)
        {
            return Ok(_dataService.getFilteredData(filterInfo));
        }
    }
}
