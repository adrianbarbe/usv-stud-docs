<template>
  <v-card>
    <v-card-title primary-title>
      <div>
        <h3 class="headline mb-0">Setari administrative</h3>
      </div>
    </v-card-title>
    <v-card-text>
      <form @submit="submitForm" novalidate="true">
        <v-flex xs12>
          <v-layout wrap>
            <v-flex xs12 class="mb-8">
              <div class="title">Anul universitar curent</div>
              <div class="caption">
                Aici puteti selecta <strong>inceputul</strong> anul universitar
                curent.
              </div>
              <v-date-picker v-model="form.educationYearStart"></v-date-picker>
            </v-flex>

            <v-flex xs12>
              <div class="title">Motivele predefinte pentru cereri</div>
              <div class="caption">
                <p>
                  Vor fi afisate studentilor. Totodata studentii vor avea
                  posibilitatea de a introduce manual motivul, daca respectivul
                  nu va fi gasit in aceasta lista.
                </p>
                <p>
                  Introduceti motivul ca text dupa care apasati Enter si veti
                  putea introduce urmatorul.
                </p>
              </div>
              <v-combobox
                v-model="form.certificateReasons"
                :items="certificateReasonsItems"
                label="Motivele predefinite pentru cereri"
                multiple
                chips
              ></v-combobox>
            </v-flex>
          </v-layout>
        </v-flex>
        <v-flex xs12> </v-flex>
        <v-card-actions>
          <v-btn
            text
            color="red"
            type="button"
            :loading="loading"
            :disabled="loading"
            >Refuza
          </v-btn>
          <v-spacer></v-spacer>
          <v-btn type="submit" :loading="loading" :disabled="loading"
            >Salveaza
          </v-btn>
        </v-card-actions>
      </form>
    </v-card-text>
  </v-card>
</template>

<script>
import AxiosService from "@/AxiosService";
import { remapErrors } from "@/helpers/errorsHelper";
import dayjs from "dayjs";
import utc from "dayjs/plugin/utc";

dayjs.extend(utc);

export default {
  name: "AdminSettingsCard",
  data: () => ({
    loading: false,

    certificateReasonsItems: [],

    form: {
      educationYearStart: null,
      certificateReasons: null,
    },
    errors: {
      educationYearStart: [],
      certificateReasons: [],
    },
  }),
  mounted() {
    this.loading = true;

    AxiosService.getInstance()
      .get("settings")
      .then((settings) => {
        if (settings.educationYearStart) {
          this.form.educationYearStart = dayjs
            .utc(settings.educationYearStart, "YYYY-MM-DD")
            .toISOString()
            .substr(0, 10);
        }

        this.form.certificateReasons = settings.certificateReasons;
        this.loading = false;
      });
  },
  methods: {
    submitForm(e) {
      e.preventDefault();

      this.loading = true;

      AxiosService.getInstance()
        .put("settings", this.form)
        .then((resp) => {
          this.loading = false;
          this.$emit("addedEdited");
        })
        .catch((err) => {
          this.loading = false;
          if (err.errors) {
            this.errors = remapErrors(err.errors);
          }
        });
    },
  },
};
</script>

<style scoped></style>
