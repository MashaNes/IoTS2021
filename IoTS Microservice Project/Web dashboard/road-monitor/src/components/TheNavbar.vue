<template>
  <div class = "navigate">
      <nav class="navigacija">
        <div class = "navigacija-beginning">
            <router-link :to = "'/'" class="navbar-item">
                <h1 class="title is-4 naziv">RoadMonitor</h1>
            </router-link>
        </div>
    </nav>
  </div>
</template>

<script>
  export default {
 
  }
</script>

<style scoped>  
    .navigate
    {
        width: 100%;
        height: 70px;
        display: flex;
        flex-direction: column-reverse;
        padding-top:15px;
    }

    .navigacija
    {
        padding: 5px;
        min-height: 70px;
        width: 100%;
        display: flex;
        flex-direction: row;
        justify-content: flex-start;
        position:sticky;
    }
    .navigacija-beginning
    {
        display: flex;
        flex-direction: row;
        flex-grow: 1;
        flex-shrink: 1;
    }
    .naziv
    {
        margin-top: 7px;
    }
    @media only screen and (max-width: 1088px)
    {
        a.navbar-item.router-link-active
        {
            padding-top: 12px;
        }
    }
</style>