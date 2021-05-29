from datetime import datetime
import time
import requests
import random
import pandas

data_microservice_location = "192.168.0.26"  # localhost
data_microservice_port = "49164"  # 49262


def two_digit_representation(value):
    if value < 10:
        return "0" + str(value)
    else:
        return str(value)


def convert_date_to_string(date):
    return str(date.year) + "-" + two_digit_representation(date.month) + "-" + two_digit_representation(date.day) + "T" \
           + two_digit_representation(date.hour) + ":" + two_digit_representation(date.minute) + ":" + \
           two_digit_representation(date.second)


class AirQualityData:
    def __init__(self, station_name, lat, long, CO_level, NMHC_level, Benzene_level, NOx_level, NO2_level, humidity):
        self.stationName = station_name
        self.latitude = lat
        self.longitude = long
        self.CO = CO_level
        self.NMHC = NMHC_level
        self.Benzene = Benzene_level
        self.NOx = NOx_level
        self.NO2 = NO2_level
        self.relativeHumidity = humidity
        timestamp = datetime.fromtimestamp(time.time())
        self.timestamp = convert_date_to_string(timestamp)

    def print(self):
        print("Timestamp: ", self.timestamp)
        print("     Station name: " + self.stationName)
        print("     Latitude: ", str(self.latitude))
        print("     Longitude: ", str(self.longitude))
        print("     CO (mg/m^3): " + str(self.CO))
        print("     Non Metanic HydroCarbons (microg/m^3): " + str(self.NMHC))
        print("     Benzene (microg/m^3): " + str(self.Benzene))
        print("     NOx (ppb): " + str(self.NOx))
        print("     NO2 (microg/m^3): " + str(self.NO2))
        print("     Relative humidity (%): " + str(self.relativeHumidity) + "\n")


class DataAcquisition:
    def __init__(self, metadata):
        self.metadata = metadata

    def send_data(self, record_data):
        url = "http://" + data_microservice_location + ":" + data_microservice_port \
              + "/api/" + self.metadata.get_metadata()["sensorType"] + "/add-data"
        values = record_data.__dict__
        headers = {"Content-type": "application/json"}
        r = requests.post(url, json=values, headers=headers)
        print(r.status_code)
        print("Sending")

    def acquisition(self):
        last_value = None

        filename = "air-quality-nis.csv"
        n = sum(1 for line in open(filename)) - 1
        s = 1
        
        time.sleep(10)

        while True:
            skip = sorted(random.sample(range(1, n + 1), n - s))
            df = pandas.read_csv(filename, skiprows=skip)
            air_data = AirQualityData(df.values[0][1], float(df.values[0][2]), float(df.values[0][3]),
                                      float(df.values[0][4]), float(df.values[0][5]), float(df.values[0][6]),
                                      float(df.values[0][7]), float(df.values[0][8]), float(df.values[0][9]))
            air_data.print()
            self.send_data(air_data)
            time.sleep(self.metadata.get_metadata()["sampleTime"])
