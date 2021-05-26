using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandMicroservice.Contracts;
using CommandMicroservice.DTOs;

namespace CommandMicroservice.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrationService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public bool RegisterDevice(RegistrationDataDTO registrationData)
        {
            string address = "http://" + registrationData.Host + ":" + registrationData.Port + "/";
            return _unitOfWork.AddClient(registrationData.StationName, address);
        }
    }
}
