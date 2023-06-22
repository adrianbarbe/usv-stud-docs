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

            <v-flex xs12>
              <div class="title">Autorizare cont pentru expediere e-mail</div>
              <div class="caption">
                <div v-if="!oAuthEmailSenderEmail">
                  <p>
                    Cu acest buton se poate autoriza contul pentru expedierea email-urilor.
                  </p>

                  <v-btn :href="getAuthUrl()" size="x-large" color="primary" class="mt-5">
                    <img width="27" :src="require('../../../assets/google.png')"/>
                    <span class="ml-2">Autorizati-va cu USV Google pentru trimitere email-uri</span>
                  </v-btn>
                </div>

                <div v-if="oAuthEmailSenderEmail">
                  <p>
                    Cu acest buton se testa expedierea email-urilor. Veti primi un email pe contul indicat.
                  </p>

                  <v-btn
                      text
                      color="primary"
                      type="button"
                      :loading="loading"
                      :disabled="loading"
                      @click="onTestEmail"
                  >Test email
                  </v-btn>

                  <h3>Email-ul autorizat pentru expediere: {{ oAuthEmailSenderEmail }}</h3>

                </div>
              </div>
            </v-flex>
          </v-layout>
        </v-flex>
        <v-flex xs12></v-flex>
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
import {remapErrors} from "@/helpers/errorsHelper";
import dayjs from "dayjs";
import utc from "dayjs/plugin/utc";
import {rolesEnum} from "@/constants/rolesEnum";

dayjs.extend(utc);

export default {
  name: "AdminSettingsCard",
  data: () => ({
    loading: false,

    certificateReasonsItems: [],

    oAuthEmailSenderEmail: null,

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

    this.getSettings();

    const queryParams = new URLSearchParams(window.location.search);
    const code = queryParams.get("code");

    if (code) {
      AxiosService.getInstance({}, {}, true)
          .post(`settings/authorizeEmailAuth`, {code: code})
          .then((_) => {
            this.$router.push({name: "adminSettings"});

            this.getSettings();
          });
    }

    this.loading = false;
  },
  methods: {
    getSettings() {
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

            this.oAuthEmailSenderEmail = settings.oAuthEmailSenderEmail;
          });
    },
    getAuthUrl() {
      const rootUrl = `https://accounts.google.com/o/oauth2/v2/auth`;

      const queryData = {
        scope: "email openid profile https://www.googleapis.com/auth/gmail.send",
        access_type: "offline",
        response_type: "code",
        redirect_uri: `${this.$config.appEndpoint}/admin/settings`,
        client_id: this.$config.oauthClientId,
        prompt: "select_account",
      };

      const qs = new URLSearchParams(queryData);

      return `${rootUrl}?${qs.toString()}`;
    },
    onTestEmail() {
      AxiosService.getInstance()
          .post("settings/testEmail")
          .then((_) => {
            this.loading = false;
          })
          .catch((_) => {
            this.loading = false;
          });
    },
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
