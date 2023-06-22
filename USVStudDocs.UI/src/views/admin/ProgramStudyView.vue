<template>
  <v-container fluid fill-height>
    <v-layout style="display: block">
      <program-study-index />
    </v-layout>
  </v-container>
</template>
<script>
import ProgramStudyIndex from "@/components/admin/programStudy/ProgramStudyIndex.vue";

export default {
  components: {
    ProgramStudyIndex,
  },
};
</script>
