from threading import Lock
from prometheus_client.core import GaugeMetricFamily, REGISTRY


class StateMetric(object):
    def __init__(self):
        self._resource_states = {}
        self._STATES = ["STARTING", "RUNNING", "STOPPING", "TERMINATED",]
        self._mutex = Lock()

    def set_state(self, resource, state):
        with self._mutex:
            self._resource_states[resource] = state

    def collect(self):
        family = GaugeMetricFamily("resource_state", "The current state of resources.",
                                   labels=["resource_state", "resource"])
        with self._mutex:
            for resource, state in self._resource_states.items():
                for s in self._STATES:
                    family.add_metric([s, resource], 1 if s == state else 0)
        yield family


sm = StateMetric()
REGISTRY.register(sm)

# Use the StateMetric.
sm.set_state("blaa", "RUNNING")
