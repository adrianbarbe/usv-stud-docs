<template>
  <v-app>
    <vue-headful title="USV Docs" />

    <navigation @toggleDrawer="drawerApp = $event" :drawer="drawerApp" />
    <toolbar @toggleDrawer="drawerApp = !drawerApp" />
    <v-main style="padding-top: 100px">
      <router-view />
    </v-main>
    <snack-displayer />
  </v-app>
</template>

<script>
import Toolbar from "@/components/user/common/Toolbar.vue";
import AdminNavigationDrawer from "@/components/common/drawers/AdminNavigationDrawer.vue";
import SnackDisplayer from "@/components/common/SnackDisplayer.vue";

export default {
  components: {
    toolbar: Toolbar,
    navigation: AdminNavigationDrawer,
    snackDisplayer: SnackDisplayer,
  },
  data: () => ({
    drawerApp: true,
  }),
};
</script>
