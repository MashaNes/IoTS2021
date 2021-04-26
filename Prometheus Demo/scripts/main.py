import http.server
import random
import time
import unittest
from prometheus_client import start_http_server, Counter, Gauge, Summary, Histogram, REGISTRY

version_info = {
    "implementation": "CPython",
    "major": "3",
    "minor": "5",
    "patchlevel": "2",
    "version": "3.5.2"
}

REQUESTS = Counter('hello_worlds_total', 'Hello Worlds requested.', labelnames=['path', 'method'])
EXCEPTIONS = Counter('hello_world_exceptions_total', 'Exceptions serving Hello World.')
SALES = Counter('hello_world_sales_euro_total', 'Euros made serving Hello World.')
INPROGRESS = Gauge('hello_worlds_inprogress', 'Number of Hello Worlds in progress.')
LAST = Gauge('hello_world_last_time_seconds', 'The last time a Hello World was served.')
LATENCY = Summary('hello_world_latency_seconds', 'Time for a request Hello World.')
LATENCYH = Histogram('hello_world_latency_histogram_seconds', 'Time for a request Hello World.',
                     buckets=[0.0001 * 2**x for x in range(1, 10)])
FOOS = Counter('foos_total', 'Number of foo calls.')
INFO = Gauge("my_python_info", "Python platform information", labelnames=version_info.keys())
INFO.labels(**version_info).set(1)


def foo():
    FOOS.inc()


class TestFoo(unittest.TestCase):
    def test_counter_inc(self):
        before = REGISTRY.get_sample_value('foos_total')
        foo()
        after = REGISTRY.get_sample_value('foos_total')
        self.assertEqual(1, after - before)


class MyHandler(http.server.BaseHTTPRequestHandler):
    # @EXCEPTIONS.count_exceptions() instead with EXCEPTIONS.count_exceptions():
    # @INPROGRESS.track_inprogress() instead INPROGRESS.inc() and INPROGRESS.dec(), takes care of functioning
    # properly when Exceptions arise
    @LATENCYH.time()  # handles latency calculation even with exception occurrence and time going backward
    def do_GET(self):
        start = time.time()
        REQUESTS.labels(self.path, self.command).inc()
        INPROGRESS.inc()
        with EXCEPTIONS.count_exceptions():
            # Code which may raise an exception
            if random.random() < 0.2:
                INPROGRESS.dec()
                raise Exception
        euros = random.random()
        SALES.inc(euros)
        self.send_response(200)
        self.end_headers()
        self.wfile.write(b"Hello World")
        LAST.set(time.time())  # LAST.set_to_current_time()
        INPROGRESS.dec()
        LATENCY.observe(time.time() - start)


if __name__ == "__main__":
    start_http_server(8000)
    server = http.server.HTTPServer(('localhost', 8001), MyHandler)
    server.serve_forever()
