using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandMicroservice.DTOs;

namespace CommandMicroservice.Contracts
{
    public interface IRegistrationService
    {
        bool RegisterDevice(RegistrationDataDTO registrationData);
    }
}
