<template>
    <div class="main-page">
        <NewEventPart class="part" />
        <TempPart class="part" />
        <EventPart class="part" />
    </div>
</template>

<script>
import EventPart from "@/components/EventPart.vue"
import NewEventPart from "@/components/NewEventPart.vue"
import TempPart from "@/components/TempPart.vue"

export default({
    components:
    {
        EventPart,
        NewEventPart,
        TempPart
    },
    methods:
    {
        subscribeToEvents() 
        {
            this.$roadMonitorHub.JoinGroup()
            this.$roadMonitorHub.$on('new_data', this.onNewData)
            window.addEventListener('beforeunload', this.leavePage)
        },
        onNewData(data)
        {
            var arg1 = ""
            var arg2 = ""
            var x = data.actuationCommand
            if(x.additionalArguments.length > 0)
                arg1 = x.additionalArguments[0]  
            if(x.additionalArguments.length > 1)
                arg2 = x.additionalArguments[1]
            data.temperatureEvent.timestamp += "+00:00"
            this.$store.state.newest_events.unshift(data.temperatureEvent)
            this.$store.state.newest_commands.unshift({
                command: x.command,
                Arg1: arg1,
                Arg2: arg2
            })
        },
        clearSignalRSubscription() {
            this.$roadMonitorHub.LeaveGroup()
            this.$roadMonitorHub.$off('new_data')
            window.removeEventListener('beforeunload', this.leavePage)
        },
        leavePage(event) {
            event.preventDefault()
            this.clearSignalRSubscription()
            event.returnValue = ''
        }
    },
    created() {
        this.subscribeToEvents()
    },
    beforeDestroy() {
      this.clearSignalRSubscription()
    }
})

</script>

<style scoped>
    .main-page
    {
        padding:15px;
        height: 100%;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        padding-bottom: 50px;
    }

    .part
    {
        padding-bottom: 50px;
        width: 100%;
    }
</style>