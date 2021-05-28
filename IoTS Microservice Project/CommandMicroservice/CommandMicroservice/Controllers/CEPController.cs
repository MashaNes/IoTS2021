using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandMicroservice.Entities;
using CommandMicroservice.Contracts;
using CommandMicroservice.DTOs;

namespace CommandMicroservice.Controllers
{
    [ApiController]
    [Route("api/cep")]
    public class CEPController : ControllerBase
    {
        private readonly INotifyService _notifyService;

        public CEPController(INotifyService notifyService)
        {
            this._notifyService = notifyService;
        }

        [HttpPost]
        [Route("add-event")]
        public async Task<ActionResult> AddEvent([FromBody] SidhhiDTO Event)
        {
            TemperatureEvent temp = new TemperatureEvent(Event.Event);
            Console.WriteLine("Received siddhi");
            await _notifyService.NotifyClient(temp);
            return Ok();
        }
    }
}
