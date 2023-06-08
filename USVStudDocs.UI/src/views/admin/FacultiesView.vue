<template>
  <v-container fluid fill-height>
    <v-layout style="display: block">
      <faculties-index />
    </v-layout>
  </v-container>
</template>
<script>
import FacultiesIndex from "@/components/admin/faculties/FacultiesIndex.vue";

export default {
  components: {
    FacultiesIndex,
  },
};
</script>
