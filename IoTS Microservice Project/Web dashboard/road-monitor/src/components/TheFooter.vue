<template>
  <footer class="futer">
    <div class="futer-top">
      <div class = "footer-main">
        <div class="divErrand deo">
          <h2 class="title is-3 belo"> RoadMonitor </h2>
          <div class="tekst"> Monitoring road conditions has never been easier</div>
        </div>
      </div>
      <div class = "footer-ikonice">
        <div class="glavne-ikonice">
          <img class="ikonica" src = "../assets/footer1.png">
          <img class="ikonica" src = "../assets/footer2.png">
          <img class="ikonica3" src = "../assets/footer3.png">
        </div>
        <div class="nav-back">
          <a @click="scrollToTop">
            <img class="ikonica skrol" src = "../assets/footerUp.png">
          </a>
        </div>
      </div>
    </div>
    <div class="futer-bottom">
      <span> © 2021 Copyright: RoadMonitor by Desmond the Moonbear </span>
    </div>
  </footer>
</template>

<script>
  export default {
    methods:
    {
      scrollToTop() {
          window.scroll({
            top: 0,
            left: 0,
            behavior: 'smooth'
          })
      }
    }
  }
</script>

<style scoped>
  .futer
  {
    background-color: rgb(62, 69, 81);
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    color:white;
  }
  .futer-bottom
  {
    background-color: rgb(0, 0, 0, 0.2);
    width: 100%;
    text-align: center;
    height: 50px;
    color: lightgrey;
    display: flex;
    flex-direction: row;
    justify-content: center;
    align-items: center;
  }
  .futer-top
  {
    width: 100%;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
  }
  .footer-ikonice
  {
    display: flex;
    flex-direction: row;
    width: 100%;
    height: 50px;
    justify-content: flex-end;
  }
  .glavne-ikonice
  {
    padding-right: 20%;
    padding-left: 20%;
    width: 100%;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
  }
  .ikonica
  {
    width: 24px;
    height: 24px;
  }
  .ikonica3
  {
    height: 34px;
  }
  .nav-back
  {
    display: flex;
    flex-direction: row;
    align-items: center;
    padding-right: 15px;
  }
  .skrol:hover
  {
    background-color: rgb(84, 94, 110);
  }
  .footer-main
  {
    width:100%;
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: space-around;
    padding-top: 20px;
    padding-bottom: 20px;
  }
  .belo
  {
    color: white;
    margin-bottom: 5px;
  }
  .divErrand
  {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
  }
  .deo
  {
    width: 45%;
    text-align: center;
  }
  .tekst
  {
    text-align: center;
    color: wheat;
    text-decoration: wavy;
  }
</style>