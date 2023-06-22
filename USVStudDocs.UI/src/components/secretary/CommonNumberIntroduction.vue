<template>
  <form @submit="submitForm" novalidate="true">
    <v-card>
      <v-card-title>
        <span class="headline">Introducerea numarului comun pentru cereri</span>
      </v-card-title>
      <v-card-text>
        <v-container grid-list-md>
          <v-layout v-if="!loading" wrap>
            <v-flex xs12>
              Numarul precedent: {{lastNumber ?? "---"}}
            </v-flex>
            <v-flex xs12>
              <v-text-field
                :error-messages="errors.number"
                v-model="form.number"
                v-mask="['###']"
                label="Numarul comun pentru ziua de astazi"
              ></v-text-field>
            </v-flex>
          </v-layout>
          <loading-placeholder v-else></loading-placeholder>
        </v-container>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn
          color="secondary"
          type="submit"
          :loading="loadingSave"
          :disabled="loadingSave"
          >Salveaza
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
        number: null,
      },
      errors: {
        number: [],
      },
      
      lastNumber: null,

      loading: false,
      loadingSave: false,
    };
  },
  mounted() {
    this.getLastNumber();
  },
  methods: {
    getLastNumber() {
      AxiosService.getInstance()
          .get(`secretaryCommonNumber/getLast`)
          .then((commonNumber) => {
            if (commonNumber.number) {
              this.lastNumber = commonNumber.number;
            }
          });
    },
    submitForm(e) {
      e.preventDefault();

      this.loadingSave = true;

      AxiosService.getInstance()
        .post(`secretaryCommonNumber`, this.form)
        .then((_) => {
          this.loadingSave = false;
          this.$emit("commonNumberAdded");
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
