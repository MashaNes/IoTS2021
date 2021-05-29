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
                            Data : "",
                            Type: "",
                            Value: "",
                            Latitude: "",
                            Longitude: "",
                            StationName: "",
                            Timestamp: ""
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
                    var arg1 = ""
                    var arg2 = ""
                    if(commands[i].AdditionalArguments.length > 0)
                        arg1 = commands[i].AdditionalArguments[0]  
                    if(commands[i].AdditionalArguments.length > 1)
                        arg2 = commands[i].AdditionalArguments[1]  
                    result.push({
                        Command: commands[i].Command,
                        Arg1: arg1,
                        Arg2: arg2
                    })
                }
                else
                    result.push(
                        {
                            Command : "",
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
        width: 44%;
    }

    .table-part1
    {
        width: 53%;
    }
</style>