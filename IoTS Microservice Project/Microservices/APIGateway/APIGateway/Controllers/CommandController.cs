using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGateway.Entities;
using APIGateway.DTOs;
using APIGateway.Contracts;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("api/command")]
    public class CommandController : ControllerBase
    {
        private readonly ICommandService _commandService;

        public CommandController(ICommandService commandService)
        {
            this._commandService = commandService;
        }

        [HttpPost]
        [Route("get-filtered-data")]
        public async Task<ActionResult> GetFilteredData([FromBody] EventCommandFilterDTO filterInfo)
        {
            return Ok(await _commandService.getFilteredData(filterInfo));
        }
    }
}
