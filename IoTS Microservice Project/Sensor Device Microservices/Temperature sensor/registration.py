import requests

command_microservice_location = "commandmicroservice"
command_microservice_port = "80"
my_location = "temp-sensorX"
my_port = "5000"


class RegistrationData:
    def __init__(self, host, port, station_name):
        self.host = host
        self.port = port
        self.StationName = station_name


def register(station_name):
    url = "http://" + command_microservice_location + ":" + command_microservice_port \
          + "/api/registration/register-device"
    info = RegistrationData(my_location, my_port, station_name)
    values = info.__dict__
    headers = {"Content-type": "application/json"}
    r = requests.post(url, json=values, headers=headers)
    print(r.status_code)
    print("Sending")
