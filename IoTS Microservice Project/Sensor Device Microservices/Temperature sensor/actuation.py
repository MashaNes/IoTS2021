import time


class Actuation:
    def __init__(self):
        self.cooling = None
        self.heating = None
        self.diagnostics_running = False
        self.tire_pressure = 36

    def get_parameters(self):
        return self.__dict__

    def turn_off_cooling(self):
        print("Cooling system off.")
        self.cooling = None

    def turn_off_heating(self):
        print("Heating system off.")
        self.heating = None

    def turn_on_cooling(self, temp):
        print("Cooling on - temperature set " + str(temp) + " degrees Celsius.")
        self.heating = None
        self.cooling = temp

    def turn_on_heating(self, temp):
        print("Heating on - temperature set " + str(temp) + " degrees Celsius.")
        self.cooling = None
        self.heating = temp

    def adjust_tire_pressure(self, value):
        print("Tire pressure adjusted. Current pressure " + str(value) + " PSI.")
        self.tire_pressure = value

    def run_diagnostics(self, location, focus):
        self.diagnostics_running = True
        if location == "road":
            print("Running outer diagnostics...")
        else:
            print("Running inner diagnostics...")
        print("Diagnostic focus on problem " + focus)
        time.sleep(2)
        print("Diagnostics complete")
        self.diagnostics_running = False
