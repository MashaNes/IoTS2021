<template>
    <div class="wrapper">
        <div class="table-part1">
            <EventTable :title="'Recent events'" :events="EventList" />
        </div>
        <div class="table-part2">
            <CommandTable :title="'Recent commands'" :commands="CommandList" />
        </div>
    </div>
</template>

<script>
import CommandTable from "@/components/CommandTable.vue"
import EventTable from "@/components/EventTable.vue"

export default {
    components:
    {
        CommandTable,
        EventTable
    },
    computed:
    {
        EventList()
        {
            var events = this.$store.state.newest_events
            var result = []
            for(var i = 0; i < 10; i++)
            {
                if(i < events.length)
                    result.push(events[i])
                else
                    result.push(
                        {
                            dataInfluenced : -1,
                            eventType: -1,
                            value: "",
                            latitude: "",
                            longitude: "",
                            stationName: "",
                            timestamp: null
                        }
                    )
            }
            return result
        },
        CommandList()
        {
            var commands = this.$store.state.newest_commands
            var result = []
            for(var i = 0; i < 10; i++)
            {
                if(i < commands.length)
                {
                   result.push(commands[i])
                }
                else
                    result.push(
                        {
                            command : "",
                            Arg1: "",
                            Arg2: ""
                        }
                    )
            }
            return result
        }
    }
}
</script>

<style scoped>
    .wrapper
    {
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: space-evenly;
    }

    .table-dark th
    {
        color: white;
    }

    .table-part2
    {
        width: 37%;
    }

    .table-part1
    {
        width: 60%;
    }
</style>