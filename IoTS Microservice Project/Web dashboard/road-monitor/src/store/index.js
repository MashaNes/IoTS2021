import Vue from 'vue'
import Vuex from 'vuex'
import router from "../router/index"
import roadMonitorHub from "../road-monitor-hub"

Vue.use(Vuex)
Vue.use(roadMonitorHub)

export default new Vuex.Store({
    state: {
        newest_events: [],
        newest_commands: [],
        temp_data: [],
        query_commands: [],
        query_events: [],
        is_temp_data_loaded: true,
        is_event_command_data_loaded: true,
        backend_host: "192.168.0.26",
        backend_port: "49156"
    },
    getters: {

    },
    mutations: {

    },
    actions: {
        getNewest()
        {
            fetch("http://"+this.state.backend_host+":"+this.state.backend_port+"/api/data/get-newest", {
                method: "GET",
                headers: {
                  "Content-type" : "application/json"
                }
              }).then(response => {
                if(response.ok) {
                  response.json().then(data => {
                    this.state.temp_data = data
                    this.state.is_temp_data_loaded = true
                  })
                }
                else {                  
                    this.state.is_temp_data_loaded = true
                    console.log(response)
                }
              })

        },
        getFilterStation({commit}, payload)
        {
            fetch("http://"+this.state.backend_host+":"+this.state.backend_port+"/api/data/get-filtered-station", {
                method: 'POST',
                mode: "cors",
                headers: {
                    "Content-type" : "application/json",
                },
                body: JSON.stringify(payload)
            }).then(response => {
                if(response.ok) {
                    response.json().then(data => {
                        this.state.temp_data = data
                        this.state.is_temp_data_loaded = true
                    })
                }
                else {
                    this.state.is_temp_data_loaded = true
                    console.log(response)
                }
            })
        },
        getFilterLocation({commit}, payload)
        {
            fetch("http://"+this.state.backend_host+":"+this.state.backend_port+"/api/data/get-filtered-location", {
                method: 'POST',
                mode: "cors",
                headers: {
                    "Content-type" : "application/json"
                },
                body: JSON.stringify(payload)
            }).then(response => {
                if(response.ok) {
                    response.json().then(data => {
                        this.state.temp_data = data
                        this.state.is_temp_data_loaded = true
                    })
                }
                else {
                    this.state.is_temp_data_loaded = true
                    console.log(response)
                }
            })
        },
        getEventCommandData({commit}, payload)
        {
            fetch("http://"+this.state.backend_host+":"+this.state.backend_port+"/api/command/get-filtered-data", {
                method: 'POST',
                mode: "cors",
                headers: {
                    "Content-type" : "application/json"
                },
                body: JSON.stringify(payload)
            }).then(response => {
                if(response.ok) {
                    response.json().then(data => {
                        this.state.query_events = data.map(x => 
                            {
                                return{
                                    dataInfluenced : x.TemperatureEvent.DataInfluenced,
                                    eventType : x.TemperatureEvent.EventType,
                                    latitude : x.TemperatureEvent.Latitude,
                                    longitude : x.TemperatureEvent.Longitude,
                                    stationName : x.TemperatureEvent.StationName,
                                    timestamp : x.TemperatureEvent.Timestamp,
                                    value : x.TemperatureEvent.Value
                                }
                            })
                        var commands = data.map(x => x.ActuationCommand)
                        this.state.query_commands = commands.map(x =>
                            {
                                var arg1 = ""
                                var arg2 = ""
                                if(x.AdditionalArguments.length > 0)
                                    arg1 = x.AdditionalArguments[0]  
                                if(x.AdditionalArguments.length > 1)
                                    arg2 = x.AdditionalArguments[1]

                                return {
                                    command: x.Command,
                                    Arg1: arg1,
                                    Arg2: arg2
                                }
                            })
                        this.state.is_event_command_data_loaded = true
                    })
                }
                else {
                    this.state.is_event_command_data_loaded = true
                    console.log(response)
                }
            })
        }
    },
    modules: {

    }
})