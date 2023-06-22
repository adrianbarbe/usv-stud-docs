<template>
  <v-app>
    <vue-headful title="USV Docs" />
    <toolbar :show-app-bar-icon="false" color="blue darken-2" />
    <v-main style="padding-top: 100px">
      <v-container fluid fill-height>
        <v-layout align-center justify-center>
          <v-flex xs12 sm8 md4>
            <p class="headline text-center">
              Sorry, we cannot find this page...
            </p>
            <p class="headline text-center">
              <router-link to="/">Navigate to Home</router-link>
            </p>
          </v-flex>
        </v-layout>
      </v-container>
    </v-main>
    <v-footer color="grey lighten-3" app>
      <v-layout xs12 wrap justify-space-between>
        <v-flex xs3 md6 class="left-footer-block">
          <v-layout wrap>
            <v-flex xs12 md2 class="caption"
              >&copy;&nbsp;{{ new Date().getFullYear() }}
            </v-flex>
            <v-flex xs12 md2 class="caption"
              >v{{ version }} | {{ build }}
            </v-flex>
            <v-flex xs12 md2 class="caption"
              >API {{ apiVersion }} | {{ apiBuild }}
            </v-flex>
          </v-layout>
        </v-flex>
        <v-flex xs9 md6 class="text-right">
          <v-layout wrap justify-space-between>
            <v-flex xs12 md4 class="caption px-1"
              >Architecture & Development: FIESC
            </v-flex>
          </v-layout>
        </v-flex>
      </v-layout>
    </v-footer>
  </v-app>
</template>

<script>
// import axios from "axios";
// import AxiosService from "@/AxiosService";

import Toolbar from "@/components/user/common/Toolbar.vue";

export default {
  data: () => ({
    // isWebview,
    version: "",
    build: "",

    apiVersion: "",
    apiBuild: "",
  }),
  components: {
    toolbar: Toolbar,
  },
  // mounted() {
  //     axios
  //         .get('/version.json')
  //         .then(resp => {
  //             if (resp) {
  //                 this.version = resp.data.version;
  //                 this.build = resp.data.build;
  //             }
  //         });
  //
  //   AxiosService.getInstance()
  //         .get('appInfo/version')
  //         .then(resp => {
  //             this.apiVersion = resp.version;
  //             this.apiBuild = resp.build;
  //         });
  // }
};
</script>
<style lang="scss">
@import "~vuetify/src/styles/styles.sass";

.left-footer-block {
  @media #{map-get($display-breakpoints, 'md-and-down')} {
    border-right: 1px solid #ccc;
  }
}
</style>
