<template>
    <div class="main-page">
        <NewEventPart class="part" />
        <TempPart class="part" />
        <EventPart class="part" />
    </div>
</template>

<script>
import EventPart from "@/components/EventPart.vue"
import NewEventPart from "@/components/NewEventPart.vue"
import TempPart from "@/components/TempPart.vue"

export default({
    components:
    {
        EventPart,
        NewEventPart,
        TempPart
    }
})

</script>

<style scoped>
    .main-page
    {
        padding:15px;
        height: 100%;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        padding-bottom: 50px;
    }

    .part
    {
        padding-bottom: 50px;
        width: 100%;
    }
</style>