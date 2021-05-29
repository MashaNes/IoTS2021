<template>
    <div class="tabela">
        <div class="title naslov"> {{title}} </div>
        <table class="table table-dark">
            <thead>
                <tr>
                    <th scope="col">#</th>
                    <th scope="col">Data</th>
                    <th scope="col">Type</th>
                    <th scope="col">Value</th>
                    <th scope="col">Latitude</th>
                    <th scope="col">Longitude</th>
                    <th scope="col">Timestamp</th>
                    <th scope="col">Station name</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(event, i) in events" :key="i">
                    <th scope="row">{{i+1}}</th>
                    <td>{{event.dataInfluenced | DataInfluencedToString}}</td>
                    <td>{{event.eventType | EventTypeToString}}</td>
                    <td>{{event.value == "" ? "" : event.value.toFixed(2)}}</td>
                    <td>{{event.latitude == "" ? "" : event.latitude.toFixed(10)}}</td>
                    <td>{{event.longitude == "" ? "" : event.longitude.toFixed(10)}}</td>
                    <td>{{event.timestamp | showTime}}</td>
                    <td>{{event.stationName}}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
export default {
    props:
    {
        events:
        {
            required: true,
            type: Array
        },
        title:
        {
            required: true,
            type: String
        }
    }
}
</script>

<style scoped>
    .tabela
    {  
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
    }   

    .naslov
    {
        width: 100%;
        text-align: center;
        margin-bottom: 15px;
    }

    .table-dark th
    {
        color: white;
    }
</style>