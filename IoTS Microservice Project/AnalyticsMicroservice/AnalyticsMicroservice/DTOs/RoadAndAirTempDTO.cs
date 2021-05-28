using System;
using System.Collections.Generic;
using System.Text;
using AnalyticsMicroservice.Entities;

namespace AnalyticsMicroservice.DTOs
{
    public class RoadAndAirTempDTO
    {
        public int RecordId { get; set; }
        public String StationName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Timestamp { get; set; }
        public double AirTemperature { get; set; }
        public double RoadTemperature { get; set; }

        public RoadAndAirTempDTO()
        { }

        public RoadAndAirTempDTO(RoadAndAirTempData data)
        {
            RecordId = data.RecordId;
            StationName = data.StationName;
            Latitude = data.Latitude;
            Longitude = data.Longitude;
            AirTemperature = data.AirTemperature;
            RoadTemperature = data.RoadTemperature;
            Timestamp = ConvertDateToString(data.Timestamp);
        }

        private static String ConvertDateToString(DateTime date)
        {
            String result = date.Year + "-" + TwoCharacterString(date.Month) + "-" + TwoCharacterString(date.Day) + "T"
                          + TwoCharacterString(date.Hour) + ":" + TwoCharacterString(date.Minute) + ":" + TwoCharacterString(date.Second);
            return result;
        }

        private static String TwoCharacterString(int number)
        {
            if (number < 10)
                return "0" + number;
            else return number.ToString();
        }
    }
}
