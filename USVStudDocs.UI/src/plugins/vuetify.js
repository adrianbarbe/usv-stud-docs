import "@mdi/font/css/materialdesignicons.css";

import Vue from "vue";
import Vuetify from "vuetify/lib/framework";
import ro from "vuetify/lib/locale/ro";

Vue.use(Vuetify);

export default new Vuetify({
  theme: {
    options: {
      customProperties: true,
    },
    themes: {
      light: {
        primary: "#264796",
        secondary: "#F47921",
        accent: "#052C6F",
        error: "#FF5252",
        info: "#2196F3",
        success: "#4CAF50",
        warning: "#FFC107",
      },
    },
  },
  lang: {
    locales: { ro },
    current: "ro",
  },
  icons: {
    iconfont: "md",
  },
});
