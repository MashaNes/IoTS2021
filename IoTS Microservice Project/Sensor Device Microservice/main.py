import random
import pandas
import time
from datetime import datetime
import ast
import json
import threading
import flask
import multiprocessing
import requests

data_microservice_location = "localhost"
data_microservice_port = "49262"

sensorType = "road_air_temp"
defaultSampleTime = 60
defaultThresh = False
defaultThreshValue = 0.1
metadataLoc = "metadata.json"
metadata = {}

try:
    metadataFile = open(metadataLoc)
    metadata = json.load(metadataFile)["metadata"]
except FileNotFoundError:
    metadataFile = open(metadataLoc, "w")
    data = {"metadata": {
        "sensorType": sensorType,
        "sampleTime": defaultSampleTime,
        "thresholding": defaultThresh,
        "threshold": defaultThreshValue
    }}
    metadata = data["metadata"]
    json.dump(data, metadataFile)
finally:
    metadataFile.close()
    print(metadata)


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


def send_data(record_data):
    url = "http://" + data_microservice_location + ":" + data_microservice_port \
          + "/api/" + metadata["sensorType"] + "/add-data"
    values = record_data.__dict__
    headers = {"Content-type": "application/json"}
    r = requests.post(url, json=values, headers=headers)
    print(r.status_code)
    print("Sending")


def background():
    last_value = None

    filename = "road-weather-information-station5.csv"
    n = sum(1 for line in open(filename)) - 1
    s = 1

    while True:
        skip = sorted(random.sample(range(1, n + 1), n - s))
        df = pandas.read_csv(filename, skiprows=skip)
        road_data = RoadData(df.StationName.values[0], ast.literal_eval(df.StationLocation.values[0])['coordinates'],
                             df.RecordId.values[0], df.RoadSurfaceTemperature.values[0], df.AirTemperature.values[0])
        road_data.print()
        if not metadata["thresholding"]:
            send_data(road_data)
        else:
            if last_value is None:
                last_value = road_data
                send_data(road_data)
            elif abs(last_value.roadTemperature - road_data.roadTemperature) < metadata["threshold"] * last_value.roadTemperature \
                    and abs(last_value.airTemperature - road_data.airTemperature) < metadata["threshold"] * last_value.airTemperature:
                last_value = road_data
                send_data(road_data)
        time.sleep(metadata["sampleTime"])


thread = threading.Thread(target=background)
thread.start()

app = flask.Flask(__name__)
lock = multiprocessing.Lock()


@app.route("/get-metadata", methods=['GET'])
def return_metadata():
    return metadata


@app.route("/set-thresholding", methods=['PUT'])
def set_thresholding():
    threshold = float(flask.request.args['threshold'])
    lock.acquire()
    metadata["thresholding"] = True
    metadata["threshold"] = threshold
    metadata_file = open(metadataLoc, "w")
    json.dump({"metadata": metadata}, metadata_file)
    metadataFile.close()
    lock.release()
    return metadata


@app.route("/no-thresholding", methods=['PUT'])
def no_thresholding():
    lock.acquire()
    metadata["thresholding"] = False
    metadata_file = open(metadataLoc, "w")
    json.dump({"metadata": metadata}, metadata_file)
    metadataFile.close()
    lock.release()
    return metadata


@app.route("/change-sample-time-seconds", methods=['PUT'])
def change_sample_time():
    new_sample_time = float(flask.request.args['sample-time'])
    lock.acquire()
    metadata["sampleTime"] = new_sample_time
    metadata_file = open(metadataLoc, "w")
    json.dump({"metadata": metadata}, metadata_file)
    metadataFile.close()
    lock.release()
    return metadata


app.run()

"""fetch("/set-thresholding?threshold=0.2", {
    method: 'PUT',
    headers: {
        "Content-type": "application/json"
    }
}).then(response => {
    response.json().then(data => {
        console.log(data)
    })
})"""
