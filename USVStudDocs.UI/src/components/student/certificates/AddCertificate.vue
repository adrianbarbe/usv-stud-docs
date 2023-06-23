<template>
  <form @submit="submitForm" novalidate="true">
    <v-card>
      <v-card-title>
        <span class="headline">Cererea unei noi adeverinte</span>
      </v-card-title>
      <v-card-text>
        <v-container grid-list-md>
          <v-layout v-if="!loading" wrap>
            <v-flex xs12>
              <v-alert
                prominent
                border="left"
                outlined
                type="info"
                elevation="2"
                class="mb-10"
              >
                <p>
                  Rugam sa selectati motivul cererii din lista predefinita, iar
                  in cazul cand nu va fi gasita una corespunzatoare se va indica
                  motivul manual.
                </p>
                <p>
                  Sa se ia in considerare ca nu puteti trimite alta cerere de
                  adeverinta pana cand cea precedenta nu va fi aprobata sau
                  respinsa.
                </p>
                <p>
                  Odata depusa cererea aici, ea poate fi aprobata sau respinsa
                  de secretarul corespunzator programului de studii. Aceasta va
                  fi vizualizat aici si de asemenea veti primi un e-mail. Daca
                  cererea este respinsa, veti primi si informatii cu privire la
                  motivul respingerii.
                </p>
                <p>
                  Odata aprobata, adeverinta va fi semnata si veti primi o
                  confirmare ca adeverinta poate fi ridiacata. In caz ca
                  confirmarea nu a fost primita dupa un timp adecvat dupa
                  aprobare, va trebui sa mergeti in persoana la secretariat sa
                  ridicati adeverinta.
                </p>
              </v-alert>
              <v-combobox
                v-model="form.reason"
                :items="reasonsList"
                :error-messages="errors.reason"
                clearable
                hide-selected
                persistent-hint
                solo
                label="Selectati din lista predefinita sau indicati manual motivul cererii"
                hint="Dupa ce ati introdus manual motivul cererii apasati tasta Enter"
              ></v-combobox>
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
          >Trimite cererea
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
    editId: Number,
    editMode: Boolean,
  },
  components: {
    LoadingPlaceholder,
  },
  data: () => {
    return {
      form: {
        reason: null,
      },
      errors: {
        reason: [],
      },
      reasonsList: [],
      loading: false,
      loadingSave: false,
    };
  },
  mounted() {
    AxiosService.getInstance()
      .get("settings/certificateReasons")
      .then((certificateReasons) => {
        this.reasonsList = certificateReasons;
      });
  },
  methods: {
    selectAllSpecialities() {
      this.form.semesters = this.semesters;
    },
    submitForm(e) {
      e.preventDefault();

      this.loadingSave = true;

      AxiosService.getInstance()
        .post("certificateStudent", this.form)
        .then((_) => {
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
