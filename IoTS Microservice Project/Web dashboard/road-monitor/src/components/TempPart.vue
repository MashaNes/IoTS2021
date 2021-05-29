<template>
    <div class="temp-part">
        <div class="title">
            Temperature data search
        </div>
        <div class="search-div">
            <select v-model="selected_option" class="selekt">
                <option v-for="(option, i) in options" :key="i" :value="i"> {{option}} </option>
            </select>
            <div v-if="selected_option == 0">
                <button class="button btn-primary dugme" :disabled="wasSearched && !isDataLoaded" @click="getNewest"> Get newest </button>
            </div>
            <div v-if="selected_option == 1" class="station">
                <label class="labela"> Station name </label>
                <input type="text" class="desno" v-model="station_search.StationName" />
                <label class="labela"> Timeframe </label>
                <input type="checkbox" class="desno" v-model="station_search.Timeframe" />
                <div v-if="station_search.Timeframe" class="station">              
                    <label class="labela"> Date and time </label>
                    <Datetime class="desno" :type="'datetime'" v-model="station_search.Datetime"/>               
                    <label class="labela"> Seconds </label>
                    <input type="number" :min="0" :step="1" class="desno smanji" v-model="station_search.Seconds" />
                </div>
                <button class="button btn-primary dugme" :disabled="disableStationButton" @click="getFilteredStation"> Get data </button>
            </div>
            <div v-if="selected_option == 2" class="station">
                <label class="labela"> Latitude </label>
                <input type="number" class="desno" v-model="location_search.Latitude" />
                <label class="labela"> Longitude </label>
                <input type="number" class="desno" v-model="location_search.Longitude" />
                <label class="labela"> Radius </label>
                <input type="number" class="desno smanji" :min="0" v-model="location_search.Radius" />
                <label class="labela"> Newest </label>
                <input type="checkbox" class="desno" v-model="location_search.Newest" />
                <button class="button btn-primary dugme" :disabled="disableLocationButton" @click="getFilteredLocation"> Get data </button>
            </div>
        </div>
        <div class="result-div">
            <span v-if="!wasSearched" class="tekst"> Nothing was searched yet </span>
            <Spinner v-if="wasSearched && !isDataLoaded"/>
            <div v-if="wasSearched && isDataLoaded">
                <span v-if="tempData.length == 0" class="tekst"> No matching results </span>
                <DataTable v-else :title="'Temperature data'" :temp_data="tempData" />
            </div>
        </div>
    </div>
</template>

<script>
import Spinner from "@/components/Spinner"
import DataTable from "@/components/DataTable"
import { Datetime } from 'vue-datetime';
import 'vue-datetime/dist/vue-datetime.css'

export default {
    components:
    {
        Spinner,
        Datetime,
        DataTable
    },
    data() {
        return{
            wasSearched: false,
            options: ["Get newest data", "Filter data by station", "Filter data by location"],
            selected_option: -1,
            station_search:
            {
                StationName: "",
                Timeframe: false,
                Datetime: this.convertDateToString(new Date(Date.now())),
                Seconds: 5
            },
            location_search:
            {
                Latitude: 0,
                Longitude: 0,
                Radius: 5,
                Newest: false
            }
        }
    },
    computed:
    {
        isDataLoaded()
        {
            return this.$store.state.is_temp_data_loaded
        },
        tempData()
        {
            return this.$store.state.temp_data
        },
        disableStationButton()
        {
            if(this.wasSearched && !this.isDataLoaded)
                return true
            if(this.station_search.StationName == "" || (this.station_search.Timeframe && this.station_search.Seconds < 0))
                return true
            return false
        },
        disableLocationButton()
        {
            if(this.wasSearched && !this.isDataLoaded)
                return true
            if(this.location_search.Radius < 0)
                return true
            return false
        }
    },
    methods:
    {
        getNewest()
        {
            this.selected_option = -1
            this.$store.state.is_temp_data_loaded = false
            this.wasSearched = true
            this.$store.dispatch("getNewest")
        },
        getFilteredStation()
        {        
            this.selected_option = -1
            this.$store.state.is_temp_data_loaded = false
            this.wasSearched = true
            var t = null
            if(this.station_search.Timeframe == true)
                t = {
                    Timestamp : this.station_search.Datetime,
                    Seconds: this.station_search.Seconds
                }
            this.$store.dispatch("getFilterStation", 
            {
                StationName: this.station_search.StationName,
                Timeframe: t
            })
        },
        getFilteredLocation()
        {
            this.selected_option = -1
            this.$store.state.is_temp_data_loaded = false
            this.wasSearched = true
            this.$store.dispatch("getFilterLocation", this.location_search)
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
        margin-left: 50px;
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
        width: 80px;
    }
</style>