<template>
  <div>
    <v-progress-linear
      v-if="loadingState > 0"
      height="3"
      style="margin: 0; z-index: 999; position: absolute"
      :indeterminate="true"
      color="primary"
    ></v-progress-linear>

    <v-app-bar
      color="primary"
      dark
      app
      height="80px"
      :clipped-left="true"
      fixed
      class="app-bar"
    >
      <v-toolbar-title
        class="ml-1 pa-1 cursor-pointer"
        @click="navigateToHome()"
      >
        <v-app-bar-nav-icon
          v-if="showAppBarIcon"
          @click.stop="$emit('toggleDrawer')"
        ></v-app-bar-nav-icon>
      </v-toolbar-title>

      <v-spacer></v-spacer>
      <div class="center-logo">
        <toolbar-logo></toolbar-logo>
      </div>
      <v-spacer></v-spacer>

      <logout-button />
    </v-app-bar>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import LogoutButton from "@/components/common/toolbar-controls/LogoutButton";
import ToolbarLogo from "@/components/common/ToolbarLogo";

export default {
  name: "ToolbarComponent",
  components: {
    LogoutButton,
    ToolbarLogo,
  },
  props: {
    showAppBarIcon: {
      type: Boolean,
      default: true,
    },
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
      this.$router.push("/");
    },
  },
};
</script>
<style lang="scss">
.invisible {
  visibility: hidden;
}
.app-bar {
  margin-bottom: 20px;
}
.center-logo {
  position: absolute;
  left: calc(50% - 45px);
}
</style>
