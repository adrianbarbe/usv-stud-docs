<template>
  <v-container fluid fill-height>
    <v-layout style="display: block">
      <faculty-personal-index />
    </v-layout>
  </v-container>
</template>
<script>
import FacultyPersonalIndex from "@/components/admin/facultyPersonal/FacultyPersonalIndex.vue";

export default {
  components: {
    FacultyPersonalIndex,
  },
};
</script>
