import json
import multiprocessing

sensorType = "road_air_temp"
defaultSampleTime = 60
defaultThresh = False
defaultThreshValue = 0.1
metadataLoc = "metadata.json"
lock = multiprocessing.Lock()


class Metadata:
    def __init__(self):
        self.metadata = {}
        metadata_file = None
        try:
            metadata_file = open(metadataLoc)
            self.metadata = json.load(metadata_file)["metadata"]
        except FileNotFoundError:
            metadata_file = open(metadataLoc, "w")
            data = {"metadata": {
                    "sensorType": sensorType,
                    "sampleTime": defaultSampleTime,
                    "thresholding": defaultThresh,
                    "threshold": defaultThreshValue
            }}
            self.metadata = data["metadata"]
            json.dump(data, metadata_file)
        finally:
            metadata_file.close()
            print(self.metadata)

    def get_metadata(self):
        return self.metadata

    def save_metadata(self):
        metadata_file = open(metadataLoc, "w")
        json.dump({"metadata": self.metadata}, metadata_file)
        metadata_file.close()

    def set_thresholding(self, value):
        lock.acquire()
        self.metadata["thresholding"] = value
        lock.release()

    def set_threshold(self, value):
        lock.acquire()
        self.metadata["threshold"] = value
        lock.release()

    def set_sample_time(self, value):
        lock.acquire()
        self.metadata["sampleTime"] = value
        lock.release()
