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
        query_events: []
    },
    getters: {

    },
    mutations: {

    },
    actions: {

    },
    modules: {

    }
})