import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'

import Vuelidate from 'vuelidate'
Vue.use(Vuelidate)

import { BootstrapVue } from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
Vue.use(BootstrapVue)

import moment from "moment"
Vue.filter("showTime", function(date) {
  if(date == null)
    return ""
  return moment(date).format("DD.MM.YYYY HH:mm:ss")
})

Vue.filter("DataInfluencedToString", function(dataInfluenced) {
  if(dataInfluenced == -1)
    return ""
  else if(dataInfluenced == 0)
    return "Road temperature"
  else
    return "Air temperature"
})

Vue.filter("EventTypeToString", function(eventType) {
  if(eventType == -1)
    return ""
  else if(eventType == 0)
    return "Hot alert"
  else if (eventType == 1)
    return "Cold alert"
  else if (eventType == 2)
    return "Temperature normal"
  else if (eventType == 3)
    return "Temperature dropped"
  else
    return "Temperature rose"
})

import roadMonitorHub from './road-monitor-hub'
Vue.use(roadMonitorHub)

Vue.config.productionTip = false

new Vue({
  router,
  store,
  Vuelidate,
  render: h => h(App)
}).$mount('#app')
