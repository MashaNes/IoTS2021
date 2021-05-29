<template>
    <div class="tabela">
        <div class="title naslov"> {{title}} </div>
            <table class="table table-dark">
                <thead>
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">Command</th>
                        <th scope="col">Args</th>
                        <th scope="col"></th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(command, i) in commands" :key="i">
                        <th scope="row">{{i+1}}</th>
                        <td>{{command.command}}</td>
                        <td>{{command.Arg1}}</td>
                        <td>{{command.Arg2}}</td>
                    </tr>
                </tbody>
            </table>
    </div>
</template>

<script>
export default {
    props:
    {
        commands:
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