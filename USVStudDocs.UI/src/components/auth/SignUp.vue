<template>
  <v-card class="sign-up-wrapper pa-10">
    <div class="d-flex align-center justify-center flex-column">
      <logo />
      <v-btn :href="getAuthUrl()" size="x-large" color="primary" class="mt-5">
        <img width="27" :src="require('../../assets/google.png')" />
        <span class="ml-2">Autorizati-va cu USV Google</span>
      </v-btn>

      <div class="mt-10 mb-2">
        By signing up or signing in you're agree with our
        <router-link :to="{ name: 'terms-and-conditions' }"
          >terms and conditions</router-link
        >
      </div>
    </div>
  </v-card>
</template>

<script>
import Logo from "@/components/common/Logo";

export default {
  name: "SignUp",
  components: {
    Logo,
  },
  methods: {
    getAuthUrl() {
      const rootUrl = `https://accounts.google.com/o/oauth2/v2/auth`;

      const queryData = {
        scope: "email openid profile",
        access_type: "offline",
        response_type: "code",
        redirect_uri: `${this.$config.appEndpoint}/auth/redirect`,
        client_id: this.$config.oauthClientId,
        prompt: "select_account",
      };

      const qs = new URLSearchParams(queryData);

      return `${rootUrl}?${qs.toString()}`;
    },
  },
};
</script>

<style scoped></style>
