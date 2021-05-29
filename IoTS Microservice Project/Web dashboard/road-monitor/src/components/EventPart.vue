<template>
    <div class="temp-part">
        <div class="title">
            Event and command data search
        </div>
        <div class="search-div">
            <label class="labela"> Station name </label>
            <input type="text" class="desno smanji" v-model="search.StationName" />
            <label class="labela"> Event type </label>
            <select v-model="search.EventType" class="selekt desno">
                <option v-for="(option, i) in EventTypes" :key="i" :value="i-1"> {{option}} </option>
            </select>
            <label class="labela"> Data influenced </label>
            <select v-model="search.DataInfluenced" class="selekt desno">
                <option v-for="(option, i) in DataInfluenced" :key="i" :value="i-1"> {{option}} </option>
            </select>
            <label class="labela"> Timeframe </label>
            <input type="checkbox" class="desno" v-model="search.Timeframe" />
            <div v-if="search.Timeframe" class="station">              
                <label class="labela"> From </label>
                <Datetime class="desno" :type="'datetime'" v-model="search.From"/>               
                <label class="labela"> To </label>
                <Datetime class="desno" :type="'datetime'" v-model="search.To"/>
            </div>
            <button class="button btn-primary dugme" :disabled="disableButton" @click="getData"> Get data </button>
        </div>
        <div class="result-div">
            <span v-if="!wasSearched" class="tekst"> Nothing was searched yet </span>
            <Spinner v-if="wasSearched && !isDataLoaded"/>
            <div v-if="wasSearched && isDataLoaded" class="full-width">
                <span v-if="eventData.length == 0" class="tekst"> No matching results </span>
                <div class="table-wrapper full-width" v-else>
                    <div class="table-part1">
                        <EventTable :title="'Searched events'" :events="eventData" />
                    </div>
                    <div class="table-part2">
                        <CommandTable :title="'Searched commands'" :commands="commandData" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import CommandTable from "@/components/CommandTable.vue"
import EventTable from "@/components/EventTable.vue"
import Spinner from "@/components/Spinner"
import { Datetime } from 'vue-datetime';
import 'vue-datetime/dist/vue-datetime.css'

export default {
    components:
    {
        Spinner,
        Datetime,
        CommandTable,
        EventTable
    },
    data() {
        return{
            wasSearched: false,
            search:
            {
                StationName: "",
                EventType: -1,
                DataInfluenced: -1,
                Timeframe: false,
                From: this.convertDateToString(new Date(Date.now())),
                To: this.convertDateToString(new Date(Date.now())),
            },
            EventTypes: ["", "Hot alert", "Cold alert", "Temperature normal", "Temperature Dropped", "Temperature rose"],
            DataInfluenced: ["", "Road temperature", "Air temperature"]
        }
    },
    computed:
    {
        isDataLoaded()
        {
            return this.$store.state.is_event_command_data_loaded
        },
        eventData()
        {
            return this.$store.state.query_events
        },
        commandData()
        {
            return this.$store.state.query_commands
        },
        disableButton()
        {
            if(this.wasSearched && !this.isDataLoaded)
                return true
            if(!this.search.Timeframe && this.search.StationName == "" && this.search.EventType == -1 && this.search.DataInfluenced == -1)
                return true
            return false
        }
    },
    methods:
    {
        getData()
        {
            this.$store.state.is_event_command_data_loaded = false
            this.wasSearched = true
            var t = null
            if(this.search.Timeframe)
                t = {
                    From: this.search.From,
                    To: this.search.To
                }
            var e = this.search.EventType == -1 ? null : this.search.EventType
            var d = this.search.DataInfluenced == -1 ? null : this.search.DataInfluenced
            var s = this.search.StationName == "" ? null : this.search.StationName
            this.$store.dispatch("getEventCommandData", 
            {
                StationName: s,
                Timeframe: t,
                EventType: e,
                DataInfluenced: d
            })
        },
        convertDateToString(date)
        {
            var year = date.getFullYear() + ''
            var month = this.twoCharString(date.getMonth() + 1)
            var day = this.twoCharString(date.getDate())
            var hours = this.twoCharString(date.getHours())
            var minutes = this.twoCharString(date.getMinutes())
            return year + "-" + month + "-" + day + "T" + hours + ":" + minutes + ":00"
        },
        twoCharString(number)
        {
            if(number > 9)
                return number + ''
            return "0" + number
        }
    }
}
</script>

<style scoped>
    .temp-part
    {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
    }

    .search-div
    {
        width:90%;
        padding: 10px;
        height:50px;
        border:1px solid black;
        border-radius: 5px;
        background-color: white;
        margin-bottom: 10px;
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: flex-start;
    }

    .station
    {
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: flex-start;
    }

    .result-div
    {
        width: 100%;
        min-height: 250px;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        padding-top: 20px;
    }

    .tekst
    {
        font-style: italic;
        font-size: 20px;
        color: grey;
    }

    .selekt
    {
        margin-left: 10px;
        margin-right: 15px;
        height: 30px;
        border-radius: 5px;
        background-color: rgb(219, 229, 233);
        padding:5px;
    }

    .dugme
    {
        height: 30px;
        margin-left: 10px;
    }

    .labela
    {
        margin-bottom: 0px;
        margin-right: 5px;
        font-weight: 600;
    }

    .desno
    {
        margin-right: 15px;
    }

    .smanji
    {
        width: 150px;
    }

    .selekt
    {
        height: 30px;
        border-radius: 5px;
        padding:5px;
    }

    .table-wrapper
    {
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: space-evenly;
    }

    .table-part2
    {
        width: 30%;
    }

    .table-part1
    {
        width: 65%;
    }

    .full-width
    {
        width: 100%;
    }
</style>