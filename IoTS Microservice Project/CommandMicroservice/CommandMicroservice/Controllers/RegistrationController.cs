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
    [Route("api/registration")]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            this._registrationService = registrationService;
        }

        [HttpPost]
        [Route("register-device")]
        public async Task<IActionResult> RegisterDevice([FromBody]RegistrationDataDTO registrationData)
        {
            this._registrationService.RegisterDevice(registrationData);
            return Ok();
        }
    }
}
