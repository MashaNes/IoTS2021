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
  return moment(date).format("DD.MM.YYYY HH:MM:SS")
})

Vue.config.productionTip = false

new Vue({
  router,
  store,
  Vuelidate,
  render: h => h(App)
}).$mount('#app')
