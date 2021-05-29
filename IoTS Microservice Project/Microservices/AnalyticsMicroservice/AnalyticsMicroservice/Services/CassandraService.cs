using System;
using System.Collections.Generic;
using System.Text;
using AnalyticsMicroservice.Contracts;
using AnalyticsMicroservice.Entities;

namespace AnalyticsMicroservice.Services
{
    public class CassandraService : ICassandraService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CassandraService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public void InsertData(TemperatureEvent temperatureEvent)
        {
            string command = "insert into " + _unitOfWork.EventTable + " (\"DataInfluenced\", \"EventType\", \"Value\", \"Latitude\", \"Longitude\", \"Timestamp\", \"StationName\")"
                            + "values ('" + temperatureEvent.DataInfluenced.ToString() + "', '" + temperatureEvent.EventType.ToString() + "', " + temperatureEvent.Value + ", "
                            + temperatureEvent.Latitude + ", " + temperatureEvent.Longitude + ", '" + ConvertDateToString(temperatureEvent.Timestamp) 
                            + "', '" + temperatureEvent.StationName + "');";
            _unitOfWork.CassandraSession.Execute(command);
        }

        private String ConvertDateToString(DateTime date)
        {
            String result = date.Year + "-" + TwoCharacterString(date.Month) + "-" + TwoCharacterString(date.Day) + "T"
                          + TwoCharacterString(date.Hour) + ":" + TwoCharacterString(date.Minute) + ":" + TwoCharacterString(date.Second);
            return result;
        }

        private String TwoCharacterString(int number)
        {
            if (number < 10)
                return "0" + number;
            else return number.ToString();
        }
    }
}
