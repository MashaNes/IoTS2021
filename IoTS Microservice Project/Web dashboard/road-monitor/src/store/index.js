import Vue from 'vue'
import Vuex from 'vuex'
import router from "../router/index"

Vue.use(Vuex)

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
        }
    },
    modules: {

    }
})