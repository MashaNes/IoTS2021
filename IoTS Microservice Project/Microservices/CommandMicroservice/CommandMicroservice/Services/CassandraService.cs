using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using CommandMicroservice.Contracts;
using CommandMicroservice.Entities;

namespace CommandMicroservice.Services
{
    public class CassandraService : ICassandraService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CassandraService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        private EventCommand ConvertCassandraRow(Row instance)
        {
            EventCommand eventCommand = new EventCommand();
            eventCommand.TemperatureEvent = new TemperatureEvent();
            eventCommand.ActuationCommand = new ActuationCommand();
            eventCommand.TemperatureEvent.Timestamp = DateTime.Parse(instance["Timestamp"].ToString());
            eventCommand.TemperatureEvent.StationName = instance["StationName"].ToString();
            eventCommand.TemperatureEvent.DataInfluenced = (DataInfluenced)Enum.Parse(typeof(DataInfluenced),instance["DataInfluenced"].ToString());
            eventCommand.TemperatureEvent.Latitude = (double)instance["Latitude"];
            eventCommand.TemperatureEvent.Longitude = (double)instance["Longitude"];
            eventCommand.TemperatureEvent.EventType = (EventType)Enum.Parse(typeof(EventType),instance["EventType"].ToString());
            eventCommand.TemperatureEvent.Value = (double)instance["Value"];
            eventCommand.ActuationCommand.Command = instance["Command"].ToString();
            List<string> args = instance["Args"].ToString().Split("&").ToList();
            eventCommand.ActuationCommand.AdditionalArguments.AddRange(args);
            return eventCommand;

            Enum.Parse(typeof(EventType), "jp");
        }

        public void InsertData(EventCommand eventCommand)
        {
            string args = "";
            foreach (string arg in eventCommand.ActuationCommand.AdditionalArguments)
            {
                args += args == "" ? "'" : "&";
                args += arg.Replace("'", "");
            }
            args += "'";

            string command = "insert into " + _unitOfWork.CassandraTable 
                            + " (\"DataInfluenced\", \"EventType\", \"Value\", \"Latitude\", \"Longitude\", \"Timestamp\", \"StationName\", \"Command\", \"Args\")"
                            + "values ('" + eventCommand.TemperatureEvent.DataInfluenced.ToString() + "', '" + eventCommand.TemperatureEvent.EventType.ToString() + "', " 
                            + eventCommand.TemperatureEvent.Value + ", " + eventCommand.TemperatureEvent.Latitude + ", " + eventCommand.TemperatureEvent.Longitude + ", '" 
                            + ConvertDateToString(eventCommand.TemperatureEvent.Timestamp) + "', '" + eventCommand.TemperatureEvent.StationName + "', '" 
                            + eventCommand.ActuationCommand.Command + "', "+ args + ");";
            _unitOfWork.CassandraSession.Execute(command);
        }

        public List<EventCommand> SelectTimeframeFilteredQuery(string table, DateTime low, DateTime high, string StationName, EventType? eventType, DataInfluenced? dataInfluenced)
        {
            string additional = StationName is not null ? " and \"StationName\"='" + StationName + "'": "";
            additional += eventType is not null ? " and \"EventType\"='" + eventType + "'" : "";
            additional += dataInfluenced is not null ? " and \"DataInfluenced\"='" + dataInfluenced + "'" : "";
            string command = "select * from " + table + " where \"Timestamp\" > '" + ConvertDateToString(low)
                           + "' and \"Timestamp\" < '" + ConvertDateToString(high) + "'" + additional +" ALLOW FILTERING";
            RowSet data = _unitOfWork.CassandraSession.Execute(command);
            return data.Select(row => ConvertCassandraRow(row)).ToList();
        }

        public List<EventCommand> SelectFilteredQuery(string table, string StationName, EventType? eventType, DataInfluenced? dataInfluenced)
        {
            string additional = StationName is not null ? "\"StationName\"='" + StationName + "'" : "";
            additional += eventType is not null ? (additional == "" ? "\"EventType\"='" + eventType + "'" : " and \"EventType\"='" + eventType + "'") : "";
            additional += dataInfluenced is not null ? (additional == "" ? "\"DataInfluenced\"='" + dataInfluenced + "'" : " and \"DataInfluenced\"='" + dataInfluenced + "'") : "";
            if (additional == "")
                return new List<EventCommand>();

            string command = "select * from " + table + " where " + additional + " ALLOW FILTERING";
            RowSet data = _unitOfWork.CassandraSession.Execute(command);
            return data.Select(row => ConvertCassandraRow(row)).ToList();
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
