using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGateway.DTOs;
using APIGateway.Contracts;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("api/data")]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dataService;

        public DataController(IDataService dataService)
        {
            this._dataService = dataService;
        }

        [HttpGet]
        [Route("get-newest")]
        public async Task<ActionResult> GetNewest()
        {
            return Ok(await _dataService.GetNewest());
        }

        [HttpPost]
        [Route("get-filtered-station")]
        public async Task<ActionResult> GetFilteredStation([FromBody] StationFilterDTO filterInfo)
        {
            return Ok(await _dataService.GetFilteredStation(filterInfo.StationName, filterInfo.Timeframe));
        }

        [HttpPost]
        [Route("get-filtered-location")]
        public async Task<ActionResult> GetFilteredLocation([FromBody] LocationFilterDTO filterInfo)
        {
            return Ok(await _dataService.GetFilteredLocation(filterInfo.Latitude, filterInfo.Longitude, filterInfo.Radius, filterInfo.Newest));
        }
    }
}
