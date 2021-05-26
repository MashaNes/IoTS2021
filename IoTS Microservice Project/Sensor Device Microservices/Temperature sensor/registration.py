import requests

command_microservice_location = "192.168.0.26"  # commandmicroservice
command_microservice_port = "49155"  # 80
my_location = "192.168.0.26"  # temp-sensor1
my_port = "5021"  # 5000


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
