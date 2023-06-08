<template>
  <v-container grid-list-md text-center>
    <v-layout row wrap justify-center>
      <v-flex xs12 md6>
        <admin-settings-card />
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
import AdminSettingsCard from "@/components/admin/settings/AdminSettingsCard.vue";

export default {
  name: "AdminSettings",
  components: {
    AdminSettingsCard,
  },
};
</script>

<style scoped></style>
