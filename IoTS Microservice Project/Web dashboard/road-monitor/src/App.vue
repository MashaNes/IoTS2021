<template>
  <div id="app">
    <TheNavbar class="please"/>
    <div class = "page-wrapper bg-light">
      <router-view />
    </div>
    <TheFooter/>
  </div>
</template>

<script>
import TheFooter from "@/components/TheFooter"
import TheNavbar from "@/components/TheNavbar"

export default {
  components: {
    TheFooter,
    TheNavbar
  },
  created()
  {
    this.startSignalR()
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

.please
{
  position: sticky;
  top: 0;
  width: 100%;
  background-color: white;
  z-index: 1000;
}

.page-wrapper
{
  min-height: 93vh;
}
</style>
