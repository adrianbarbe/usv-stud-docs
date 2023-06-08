import Vue from "vue";
import jwtDecode from "jwt-decode";
import intersectionWith from "lodash/intersectionWith";

let auth = new Vue({
  created() {},
  computed: {
    bearerToken: {
      get: () => {
        return localStorage.getItem("bearer_token");
      },
      set: (token) => {
        localStorage.setItem("bearer_token", token);
      },
    },
    refreshToken: {
      get: () => {
        return localStorage.getItem("refresh_token");
      },
      set: (token) => {
        localStorage.setItem("refresh_token", token);
      },
    },
    authToken: {
      get: () => {
        return JSON.parse(localStorage.getItem("auth_token"));
      },
      set: (token) => {
        localStorage.setItem("auth_token", JSON.stringify(jwtDecode(token)));
      },
    },
  },
  methods: {
    login(token, refreshToken) {
      this.bearerToken = token;
      this.refreshToken = refreshToken;

      this.authToken = token;
    },
    logout() {
      localStorage.removeItem("bearer_token");
      localStorage.removeItem("auth_token");
      localStorage.removeItem("refresh_token");
    },
    isAuthenticated() {
      const authToken = JSON.parse(localStorage.getItem("auth_token"));
      if (authToken === null) {
        return false;
      }

      return new Date().getTime() / 1000 < authToken.exp;
    },
    getCurrentUser() {
      const authToken = JSON.parse(localStorage.getItem("auth_token"));
      return authToken !== null && authToken.unique_name;
    },
    getRole() {
      const authToken = JSON.parse(localStorage.getItem("auth_token"));

      if (authToken !== null) {
        return authToken.role;
      }

      return null;
    },
    getUsername() {
      const idToken = localStorage.getItem("bearer_token");
      if (idToken === null || idToken === "undefined") {
        return false;
      }

      const decoded = jwtDecode(idToken);

      return decoded !== null && decoded.unique_name;
    },
    hasRoles(rolesStr) {
      let role = typeof rolesStr === "string" ? [rolesStr] : rolesStr;

      return (
        intersectionWith(role, this.getRoles(), (a, b) => {
          return a === b;
        }).length > 0
      );
    },
    getBearerToken() {
      return localStorage.getItem("bearer_token");
    },
    getRefreshToken() {
      return localStorage.getItem("refresh_token");
    },
  },
});

export default {
  install: function (Vue) {
    Vue.prototype.$auth = auth;
  },
};
