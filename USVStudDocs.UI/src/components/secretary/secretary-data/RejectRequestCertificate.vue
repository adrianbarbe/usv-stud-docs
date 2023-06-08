<template>
  <form @submit="submitForm" novalidate="true">
    <v-card>
      <v-card-title>
        <span class="headline">Refuzul cererii</span>
      </v-card-title>
      <v-card-text>
        <v-container grid-list-md>
          <v-layout v-if="!loading" wrap>
            <v-flex xs12>
              <v-text-field
                :error-messages="errors.denyReason"
                v-model="form.denyReason"
                label="Motivul de refuz pentru cererea de certificat"
              ></v-text-field>
            </v-flex>
          </v-layout>
          <loading-placeholder v-else></loading-placeholder>
        </v-container>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn
          color="primary"
          text
          @click.native="$emit('shouldCloseRefuse')"
          :disabled="loadingSave"
          >Refuza
        </v-btn>
        <v-btn
          color="secondary"
          type="submit"
          :loading="loadingSave"
          :disabled="loadingSave"
          >Refuza cererea de certificat
        </v-btn>
      </v-card-actions>
    </v-card>
  </form>
</template>

<script>
import AxiosService from "@/AxiosService";
import { remapErrors } from "@/helpers/errorsHelper";
import LoadingPlaceholder from "@/components/common/LoadingPlaceholder.vue";
import fieldOfStudy from "@/constants/fieldOfStudyEnum";

export default {
  props: {
    itemId: Number,
  },
  components: {
    LoadingPlaceholder,
  },
  data: () => {
    return {
      form: {
        denyReason: null,
      },
      errors: {
        denyReason: [],
      },
      reasonsList: [],
      loading: false,
      loadingSave: false,
    };
  },
  methods: {
    submitForm(e) {
      e.preventDefault();

      this.loadingSave = true;

      AxiosService.getInstance()
        .put(`certificateSecretary/reject/${this.itemId}`, this.form)
        .then((_) => {
          this.loadingSave = false;
          this.$emit("rejected");
        })
        .catch((err) => {
          this.loadingSave = false;
          if (err.errors) {
            this.errors = remapErrors(err.errors);
          }
        });
    },
  },
};
</script>
