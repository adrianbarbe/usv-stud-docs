import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

import snackMessages from "./store/admin/snackMessages";
import topNavigation from "./store/admin/topNavigation";
import loadingState from "./store/loadingState";

export default new Vuex.Store({
  modules: {
    snackMessages,
    loadingState,
    topNavigation,
  },
  state: {},
  mutations: {},
  actions: {},
});
