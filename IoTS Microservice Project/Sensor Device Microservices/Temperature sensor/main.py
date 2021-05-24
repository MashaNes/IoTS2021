import threading
import flask
from flask_cors import CORS, cross_origin
from metadata import Metadata
from data_acqusition import DataAcquisition

metadata = Metadata()
data_acquisition = DataAcquisition(metadata)

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
