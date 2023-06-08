<template>
  <div>
    <v-progress-linear
      v-if="loadingState > 0"
      height="3"
      style="margin: 0; z-index: 999; position: absolute"
      :indeterminate="true"
      color="info"
    ></v-progress-linear>

    <v-app-bar
      color="red darken-1"
      dark
      app
      height="50px"
      :clipped-left="$vuetify.breakpoint.mdAndUp"
      fixed
    >
      <v-toolbar-title
        style="width: 250px"
        class="ml-0 pl-0 cursor-pointer"
        @click="navigateToHome()"
      >
        <v-app-bar-nav-icon
          @click.stop="$emit('toggleDrawer')"
        ></v-app-bar-nav-icon>
        <toolbar-logo></toolbar-logo>
      </v-toolbar-title>

      <v-spacer></v-spacer>

      <v-toolbar-title>{{ getTopNavigationTitle }}</v-toolbar-title>

      <v-spacer></v-spacer>

      <quick-navigation />
      <logout-button />
    </v-app-bar>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import LogoutButton from "../../common/toolbar-controls/LogoutButton";
import QuickNavigation from "../../common/toolbar-controls/QuickNavigation";
import ToolbarLogo from "../../common/ToolbarLogo";

export default {
  name: "ToolbarComponent",
  components: {
    LogoutButton,
    QuickNavigation,
    ToolbarLogo,
  },
  computed: {
    ...mapGetters(["selectedNavigationItem", "getTopNavigationTitle"]),
    loadingState() {
      return this.$store.getters["loadingState/loadingState"];
    },
  },
  methods: {
    isRouteWithNav() {
      return this.$route.params.group !== undefined;
    },
    navigateToHome() {
      this.$router.push("/").catch((_) => {});
    },
  },
};
</script>
<style lang="scss">
.invisible {
  visibility: hidden;
}
</style>
