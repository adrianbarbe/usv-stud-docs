<template>
  <v-card>
    <v-card-title class="title">Stergerea</v-card-title>
    <v-card-text>
      Sunteti increzuti ca doriti sa stergeti acest program de studii?
    </v-card-text>
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-btn
        color="primary"
        @click.native="$emit('shouldCloseDeleteConfirm')"
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
    deleteId: Number,
  },
  methods: {
    removeItem() {
      this.loadingDelete = true;

      AxiosService.getInstance()
        .delete(`programStudy/${this.deleteId}`)
        .then((_) => {
          this.loadingDelete = false;

          this.$emit("removed");
        });
    },
  },
};
</script>
