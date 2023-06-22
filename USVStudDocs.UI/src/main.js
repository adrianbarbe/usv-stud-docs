import Vue from "vue";
import Vuex from "vuex";
import "material-design-icons-iconfont/dist/material-design-icons.css";
import router from "./router";
import store from "./store";
import AuthJwt from "./plugins/auth-jwt";
import vueHeadful from "vue-headful";
import VueTheMask from "vue-the-mask";

import "./styles/main.scss";

import ConfigFactory, { AppConfig } from "./config/configFactory";
import config from "./app.config";

import App from "./App.vue";
import vuetify from "./plugins/vuetify";
import VueAnalytics from "vue-analytics";

import { loadFonts } from "./plugins/webfontloader";

loadFonts();

let configuration = AppConfig(config);

// const app = createApp(App);
// const router = routerInstance(app);
//
// app.use(ConfigFactory, config)
//     .use(AuthJwt)
//     .use(vuetify)
//     .use(router)
//     .use(store)
//     .mount('#app');

Vue.component("vue-headful", vueHeadful);

Vue.use(VueAnalytics, {
  id: configuration.GA.id,
  router,
});

Vue.use(VueTheMask);
Vue.use(ConfigFactory, config);
Vue.use(Vuex);
Vue.use(AuthJwt);

Vue.config.productionTip = false;

new Vue({
  router,
  store,
  vuetify,
  render: (h) => h(App),
}).$mount("#app");
