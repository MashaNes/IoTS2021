using System;
using System.Collections.Generic;
using System.Text;
using AnalyticsMicroservice.Contracts;
using AnalyticsMicroservice.Entities;

namespace AnalyticsMicroservice.Services
{
    struct Temperatures
    {
        public Temperatures(double RoadTemp, double AirTemp)
        {
            this.RoadTemp = RoadTemp;
            this.AirTemp = AirTemp;
        }

        public double RoadTemp;
        public double AirTemp;
    }

    public class AnalyticsService : IAnalyticsService
    {
        private double _upperBound = 30;
        private double _lowerBound = 0;
        private double _risingPercentage = 1.1;
        private double _fallingPercentage = 0.9;

        private Dictionary<String, Temperatures> LasKnownValues = new Dictionary<string, Temperatures>();
        private Dictionary<String, EventType> FiringEvents = new Dictionary<string, EventType>();

        public List<TemperatureEvent> AnalyseNewData(RoadAndAirTempData data)
        {
            List<TemperatureEvent> events = new List<TemperatureEvent>();
            events.AddRange(CheckExtreme(data, DataInfluenced.RoadTemperature));
            events.AddRange(CheckExtreme(data, DataInfluenced.AirTemperature));

            if(LasKnownValues.ContainsKey(data.StationName))
            {
                Temperatures temps = LasKnownValues[data.StationName];
                
                TemperatureEvent Event = CheckSudden(data, DataInfluenced.RoadTemperature, temps.RoadTemp);
                if (Event != null)
                    events.Add(Event);

                Event = CheckSudden(data, DataInfluenced.AirTemperature, temps.AirTemp);
                if (Event != null)
                    events.Add(Event);
            }

            if (LasKnownValues.ContainsKey(data.StationName))
                LasKnownValues[data.StationName] = new Temperatures(data.RoadTemperature, data.AirTemperature);
            else
                LasKnownValues.Add(data.StationName, new Temperatures(data.RoadTemperature, data.AirTemperature));
            
            return events;
        }

        private List<TemperatureEvent> CheckExtreme(RoadAndAirTempData data, DataInfluenced dataInfluenced)
        {
            List<TemperatureEvent> events = new List<TemperatureEvent>();
            double temp = dataInfluenced == DataInfluenced.RoadTemperature ? data.RoadTemperature : data.AirTemperature;
            if (FiringEvents.ContainsKey(data.StationName + "_" + dataInfluenced.ToString()))
            {
                EventType eventType = FiringEvents[data.StationName + "_" + dataInfluenced.ToString()];
                if (temp >= _lowerBound && temp <= _upperBound)
                {
                    events.Add(BuildEvent(data, dataInfluenced, EventType.TempNormalized));
                    FiringEvents.Remove(data.StationName + "_" + dataInfluenced.ToString());
                }
                else if (eventType == EventType.HotAlert && temp < _lowerBound)
                {
                    FiringEvents[data.StationName + "_" + dataInfluenced.ToString()] = EventType.ColdAlert;
                    events.Add(BuildEvent(data, dataInfluenced, EventType.ColdAlert));
                }
                else if (eventType == EventType.ColdAlert && temp > _upperBound)
                {
                    FiringEvents[data.StationName + "_" + dataInfluenced.ToString()] = EventType.HotAlert;
                    events.Add(BuildEvent(data, dataInfluenced, EventType.HotAlert));
                }
            }
            else
            {
                if (temp > _upperBound)
                {
                    FiringEvents.Add(data.StationName + "_" + dataInfluenced.ToString(), EventType.HotAlert);
                    events.Add(BuildEvent(data, dataInfluenced, EventType.HotAlert));
                }
                else if (temp < _lowerBound)
                {

                    FiringEvents.Add(data.StationName + "_" + dataInfluenced.ToString(), EventType.ColdAlert);
                    events.Add(BuildEvent(data, dataInfluenced, EventType.ColdAlert));
                }
            }
            return events;
        }

        private TemperatureEvent CheckSudden(RoadAndAirTempData data, DataInfluenced dataInfluenced, double oldValue)
        {
            double newValue = dataInfluenced == DataInfluenced.RoadTemperature ? data.RoadTemperature : data.AirTemperature;
            if (newValue > _risingPercentage * oldValue)
                return BuildEvent(data, dataInfluenced, EventType.TempRiseAlert);
            if (newValue < _fallingPercentage * oldValue)
                return BuildEvent(data, dataInfluenced, EventType.TempDropAlert);
            return null;
        }

        private TemperatureEvent BuildEvent(RoadAndAirTempData data, DataInfluenced dataInfluenced, EventType alert)
        {
            return new TemperatureEvent()
            {
                DataInfluenced = dataInfluenced,
                EventType = alert,
                Timestamp = data.Timestamp,
                Latitude = data.Latitude,
                Longitude = data.Longitude,
                StationName = data.StationName,
                Value = dataInfluenced == DataInfluenced.RoadTemperature ? data.RoadTemperature : data.AirTemperature
            };
        }
    }
}
