import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

export default {
    install (Vue) { 
      const roadMonitorHub = new Vue()
      Vue.prototype.$roadMonitorHub = roadMonitorHub
  
      let connection = null
      let startedPromise = null
      let manuallyClosed = false
  
      Vue.prototype.startSignalR = () => {
        connection = new HubConnectionBuilder()
          .withUrl("http://192.168.0.26:49156/road-monitor-hub")
          .configureLogging(LogLevel.Debug)
          .build()
  
        
        addSignalREventListener('new_data')
  
        function addSignalREventListener(name) {
          connection.on(name, (payload) => {
            roadMonitorHub.$emit(name, payload)
          })
        }
  
        function start () {
          startedPromise = connection.start()
            .catch(err => {
              console.error('Failed to connect with hub', err)
              return new Promise((resolve, reject) => setTimeout(() => start().then(resolve).catch(reject), 5000))
            })
          return startedPromise
        }
  
        connection.onclose(() => {
          if (!manuallyClosed) start()
        })
  
        manuallyClosed = false
        start()
      }
  
      Vue.prototype.stopSignalR = () => {
        if (!startedPromise) return
  
        manuallyClosed = true
        return startedPromise
          .then(() => connection.stop())
          .then(() => { startedPromise = null })
      }
  
      roadMonitorHub.JoinGroup = () => {
        if (!startedPromise) return
  
        return startedPromise
          .then(() => connection.invoke('JoinGroup'))
          .catch(console.error)
      }
  
      roadMonitorHub.LeaveGroup = () => {
        if(!startedPromise) return
  
        return startedPromise
        .then(() => connection.invoke('LeaveGroup'))
        .catch(console.error)
      }
    }
  }