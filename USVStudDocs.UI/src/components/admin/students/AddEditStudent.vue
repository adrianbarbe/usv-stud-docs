<template>
  <form @submit="submitForm" novalidate="true">
    <v-card>
      <v-card-title>
        <span class="headline">{{ editMode ? "Editarea" : "Adauga" }}</span>
      </v-card-title>
      <v-card-text>
        <v-container grid-list-md>
          <v-layout v-if="!loading" wrap>
            <v-flex xs12>
              <v-text-field
                :error-messages="errors.surname"
                v-model="form.surname"
                label="Nume"
              ></v-text-field>
            </v-flex>
            <v-flex xs12>
              <v-text-field
                :error-messages="errors.name"
                v-model="form.name"
                label="Prenume"
              ></v-text-field>
            </v-flex>
            <v-flex xs12>
              <v-text-field
                :error-messages="errors.patronymic"
                v-model="form.patronymic"
                label="Initiala tatalui"
              ></v-text-field>
            </v-flex>
            <v-flex xs12>
              <v-text-field
                :error-messages="errors.email"
                v-model="form.email"
                label="Email"
              ></v-text-field>
            </v-flex>
            <v-flex xs12>
              <v-radio-group
                :error-messages="errors.gender"
                v-model="form.gender"
                mandatory
              >
                <v-radio label="Masculin" :value="0"></v-radio>
                <v-radio label="Feminin" color="red" :value="1"></v-radio>
              </v-radio-group>
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
          @click.native="$emit('shouldCloseAddEdit')"
          :disabled="loadingSave"
          >Refuza
        </v-btn>
        <v-btn
          color="primary"
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
import { remapErrors } from "@/helpers/errorsHelper";
import AxiosService from "@/AxiosService";
import LoadingPlaceholder from "@/components/common/LoadingPlaceholder.vue";

export default {
  props: {
    editId: Number,
    editMode: Boolean,
    items: Array,
  },
  components: {
    LoadingPlaceholder,
  },
  data: () => {
    return {
      form: {
        surname: "",
        name: "",
        patronymic: "",
        email: "",
      },
      errors: {
        surname: [],
        name: [],
        patronymic: [],
        email: [],
      },
      loading: false,
      loadingSave: false,
    };
  },
  mounted() {
    if (this.editMode) {
      this.loading = true;

      AxiosService.getInstance()
        .get(`student/${this.editId}`)
        .then((resp) => {
          this.loading = false;
          this.form = resp;
        });
    }
  },
  methods: {
    submitForm(e) {
      e.preventDefault();

      this.loadingSave = true;

      let additionalParams = {
        faculty: { id: this.$route.params.faculty },
        yearSemester: { id: this.$route.params.semester },
        programStudy: { id: this.$route.params.speciality },
      };

      AxiosService.getInstance()
        .post("student", { ...this.form, ...additionalParams })
        .then((resp) => {
          this.loadingSave = false;
          this.$emit("addedEdited");
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
