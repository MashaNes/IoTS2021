from datetime import datetime
import time
import requests
import random
import pandas
import ast

data_microservice_location = "datamicroservice"  # localhost
data_microservice_port = "80"  # 49262


def two_digit_representation(value):
    if value < 10:
        return "0" + str(value)
    else:
        return str(value)


def convert_date_to_string(date):
    return str(date.year) + "-" + two_digit_representation(date.month) + "-" + two_digit_representation(date.day) + "T"\
           + two_digit_representation(date.hour) + ":" + two_digit_representation(date.minute) + ":" + \
           two_digit_representation(date.second)


class RoadData:
    def __init__(self, name, location, record_id, road_temp, air_temp):
        self.stationName = name
        self.latitude = location[1]
        self.longitude = location[0]
        self.recordId = int(record_id)
        self.roadTemperature = road_temp
        self.airTemperature = air_temp
        timestamp = datetime.fromtimestamp(time.time())
        self.timestamp = convert_date_to_string(timestamp)

    def print(self):
        print("Record id: " + str(self.recordId))
        print("     Station name: " + self.stationName)
        print("     Latitude: ", str(self.latitude))
        print("     Longitude: ", str(self.longitude))
        print("     Timestamp: ", self.timestamp)
        print("     Road surface temperature: " + str(self.roadTemperature))
        print("     Air temperature: " + str(self.airTemperature) + "\n")


class DataAcquisition:
    def __init__(self, metadata):
        self.metadata = metadata
        self.filename = "road-weather-information-station.csv"
        self.n = sum(1 for line in open(self.filename)) - 1
        self.s = 1

    def send_data(self, record_data):
        url = "http://" + data_microservice_location + ":" + data_microservice_port \
              + "/api/" + self.metadata.get_metadata()["sensorType"] + "/add-data"
        values = record_data.__dict__
        headers = {"Content-type": "application/json"}
        r = requests.post(url, json=values, headers=headers)
        print(r.status_code)
        print("Sending")

    def get_station_name(self):
        skip = sorted(random.sample(range(1, self.n + 1), self.n - self.s))
        df = pandas.read_csv(self.filename, skiprows=skip)
        return df.StationName.values[0]

    def acquisition(self):
        last_value = None
        time.sleep(10)

        while True:
            skip = sorted(random.sample(range(1, self.n + 1), self.n - self.s))
            df = pandas.read_csv(self.filename, skiprows=skip)
            road_data = RoadData(df.StationName.values[0],
                                 ast.literal_eval(df.StationLocation.values[0])['coordinates'],
                                 df.RecordId.values[0], df.RoadSurfaceTemperature.values[0],
                                 df.AirTemperature.values[0])
            road_data.print()
            if not self.metadata.get_metadata()["thresholding"]:
                self.send_data(road_data)
            else:
                if last_value is None:
                    last_value = road_data
                    self.send_data(road_data)
                elif abs(last_value.roadTemperature - road_data.roadTemperature) \
                        > self.metadata.get_metadata()["threshold"] * last_value.roadTemperature \
                        and abs(last_value.airTemperature - road_data.airTemperature) \
                        > self.metadata.get_metadata()["threshold"] * last_value.airTemperature:
                    last_value = road_data
                    self.send_data(road_data)
            time.sleep(self.metadata.get_metadata()["sampleTime"])
