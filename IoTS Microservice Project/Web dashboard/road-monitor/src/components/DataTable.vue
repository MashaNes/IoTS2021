<template>
    <div class="tabela">
        <div class="title naslov"> {{title}} </div>
        <table class="table table-dark">
            <thead>
                <tr>
                    <th scope="col">#</th>
                    <th scope="col">Record id</th>
                    <th scope="col">Station name</th>
                    <th scope="col">Latitude</th>
                    <th scope="col">Longitude</th>
                    <th scope="col">Timestamp</th>
                    <th scope="col">Air temperature</th>
                    <th scope="col">Road temperature</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(temp, i) in temp_data" :key="i">
                    <th scope="row">{{i+1}}</th>
                    <td>{{temp.RecordId}}</td>
                    <td>{{temp.StationName}}</td>
                    <td>{{temp.Latitude}}</td>
                    <td>{{temp.Longitude}}</td>
                    <td>{{temp.Timestamp | showTime}}</td>
                    <td>{{temp.AirTemperature}}</td>
                    <td>{{temp.RoadTemperature}}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
export default {
    props:
    {
        temp_data:
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