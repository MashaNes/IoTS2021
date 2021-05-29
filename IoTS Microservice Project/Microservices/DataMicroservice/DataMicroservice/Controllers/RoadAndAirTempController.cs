using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMicroservice.Entities;
using DataMicroservice.Contracts;
using DataMicroservice.DTOs;

namespace DataMicroservice.Controllers
{
    [ApiController]
    [Route("api/road_air_temp")]
    public class RoadAndAirTempController : ControllerBase
    {
        private readonly ITempService _tempService;

        public RoadAndAirTempController(ITempService tempService)
        {
            _tempService = tempService;
        }

        [HttpPost]
        [Route("add-data")]
        public async Task<ActionResult> AddData([FromBody]RoadAndAirTempData roadData)
        {
            await _tempService.AddData(roadData);
            return Ok();
        }

        [HttpGet]
        [Route("get-data")]
        public async Task<ActionResult> GetData()
        {
            List<RoadAndAirTempData> retVal = await _tempService.GetAllData();
            return Ok(retVal);
        }

        [HttpGet]
        [Route("get-newest")]
        public async Task<ActionResult> GetNewest()
        {
            List<RoadAndAirTempData> retVal = await _tempService.GetNewest();
            return Ok(retVal);
        }

        [HttpGet]
        [Route("get-data-recordId/{RecordId}")]
        public async Task<ActionResult> GetData(int RecordId)
        {
            RoadAndAirTempData retVal = await _tempService.GetDataByRecordId(RecordId);
            return Ok(retVal);
        }

        [HttpGet]
        [Route("get-data-station/{StationName}/{Newest}")]
        public async Task<ActionResult> GetDataStation(String StationName, bool Newest)
        {
            List<RoadAndAirTempData> retVal = await _tempService.GetDataByStationName(StationName, Newest);
            return Ok(retVal);
        }

        [HttpPost]
        [Route("get-data-timeframe")]
        public async Task<ActionResult> GetDataTimeframe([FromBody]TimeframeDTO timeframe)
        {
            List<RoadAndAirTempData> retVal = await _tempService.GetDataByTimestamp(timeframe.StationName, timeframe.ReferentTime, timeframe.TimeframeSeconds);
            return Ok(retVal);
        }

        [HttpPost]
        [Route("get-data-location")]
        public async Task<ActionResult> GetDataLocation([FromBody] LocationRadiusDTO locationRadius)
        {
            List<RoadAndAirTempData> retVal = await _tempService.GetDataLocation(locationRadius);
            return Ok(retVal);
        }
    }
}
