<template>
  <v-container fluid fill-height>
    <v-layout style="display: block">
      <admin-settings />
    </v-layout>
  </v-container>
</template>
<script>
import AdminSettings from "@/components/admin/settings/AdminSettings.vue";

export default {
  components: {
    AdminSettings,
  },
};
</script>
