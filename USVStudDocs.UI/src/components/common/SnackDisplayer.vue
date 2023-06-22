<template>
  <v-snackbar v-model="show" :color="color" :multi-line="true" :timeout="30000">
    {{ message }}

    <template v-slot:action="{ attrs }">
      <v-btn
        dark
        text
        v-bind="attrs"
        @click.native="$store.commit('snackMessages/update', { show: false })"
      >
        <v-icon>clear</v-icon>
      </v-btn>
    </template>
  </v-snackbar>
</template>
<script>
export default {
  computed: {
    message() {
      return this.$store.getters["snackMessages/message"];
    },
    color() {
      return this.$store.getters["snackMessages/color"];
    },
    show: {
      get() {
        return this.$store.getters["snackMessages/show"];
      },
      set(value) {
        this.$store.commit("snackMessages/update", { show: value });
      },
    },
  },
};
</script>
