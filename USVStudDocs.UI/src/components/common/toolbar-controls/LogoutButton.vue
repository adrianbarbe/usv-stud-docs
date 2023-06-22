<template>
  <div>
    <v-menu v-if="$auth.isAuthenticated()" offset-y left>
      <template v-slot:activator="{ on }">
        <v-btn v-on="on" color="info">
          {{ $auth.getUsername() }}
        </v-btn>
      </template>
      <v-list>
        <v-list-item @click="logout()">
          <v-list-item-title>Iesire din cont</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>
    <v-btn v-if="!$auth.isAuthenticated()" @click="login()" color="secondary">
      Intrare in cont
    </v-btn>
  </div>
</template>
<script>
export default {
  methods: {
    logout() {
      this.$router.push({ name: "sign-up" });
      this.$auth.logout();
    },
  },
};
</script>
