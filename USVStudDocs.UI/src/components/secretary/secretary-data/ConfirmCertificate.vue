<template>
  <v-card>
    <v-card-title class="title">Confirmarea</v-card-title>
    <v-card-text>
      Sunteti increzuti ca doriti sa conirmati aceasta cerere de certificat?
    </v-card-text>
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-btn
        color="primary"
        @click.native="$emit('shouldCloseConfirm')"
        :disabled="loadingDelete"
        >Nu
      </v-btn>
      <v-btn
        text
        @click.native="removeItem"
        :loading="loadingDelete"
        :disabled="loadingDelete"
        >Da
      </v-btn>
    </v-card-actions>
  </v-card>
</template>
<script>
import AxiosService from "@/AxiosService";

export default {
  data: () => {
    return {
      loadingDelete: false,
    };
  },
  props: {
    itemId: Number,
  },
  methods: {
    removeItem() {
      this.loadingDelete = true;

      AxiosService.getInstance()
        .put(`certificateSecretary/confirm/${this.itemId}`)
        .then((_) => {
          this.loadingDelete = false;

          this.$emit("confirmed");
        });
    },
  },
};
</script>
