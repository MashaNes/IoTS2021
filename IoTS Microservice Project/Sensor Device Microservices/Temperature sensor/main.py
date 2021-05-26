import threading
import time
import flask
from flask_cors import CORS, cross_origin
from metadata import Metadata
from data_acqusition import DataAcquisition
import registration
from actuation import Actuation

metadata = Metadata()
data_acquisition = DataAcquisition(metadata)
actuation = Actuation()

thread = threading.Thread(target=data_acquisition.acquisition)
thread.start()

app = flask.Flask(__name__)
cors = CORS(app)


@app.route("/get-metadata", methods=['GET'])
@cross_origin()
def return_metadata():
    return metadata.get_metadata()


@app.route("/set-thresholding", methods=['PUT'])
@cross_origin()
def set_thresholding():
    threshold = float(flask.request.args['threshold'])
    metadata.set_thresholding(True)
    metadata.set_threshold(threshold)
    metadata.save_metadata()
    return metadata.get_metadata()


@app.route("/no-thresholding", methods=['PUT'])
@cross_origin()
def no_thresholding():
    metadata.set_thresholding(False)
    metadata.save_metadata()
    return metadata.get_metadata()


@app.route("/change-sample-time-seconds", methods=['PUT'])
@cross_origin()
def change_sample_time():
    new_sample_time = float(flask.request.args['sample-time'])
    metadata.set_sample_time(new_sample_time)
    metadata.save_metadata()
    return metadata.get_metadata()


@app.route("/normalized-temperature", methods=['PUT'])
@cross_origin()
def normalized_temperature():
    actuation.turn_off_cooling()
    actuation.turn_off_heating()
    return actuation.get_parameters()


@app.route("/start-cooling", methods=['PUT'])
@cross_origin()
def start_cooling():
    temperature = float(flask.request.args['temp'])
    actuation.turn_on_cooling(temperature)
    return actuation.get_parameters()


@app.route("/start-heating", methods=['PUT'])
@cross_origin()
def start_heating():
    temperature = float(flask.request.args['temp'])
    actuation.turn_on_heating(temperature)
    return actuation.get_parameters()


@app.route("/start-diagnostics", methods=['PUT'])
@cross_origin()
def start_diagnostics():
    location = flask.request.args['loc']
    event = flask.request.args['event']
    actuation.run_diagnostics(location, event)
    return actuation.get_parameters()


@app.route("/adjust-tire-pressure", methods=['PUT'])
@cross_origin()
def adjust_tire_pressure():
    psi = float(flask.request.args['psi'])
    actuation.adjust_tire_pressure(psi)
    return actuation.get_parameters()


time.sleep(100)
registration.register(data_acquisition.get_station_name())
app.run()
