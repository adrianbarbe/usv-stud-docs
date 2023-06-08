<template>
  <form @submit="submitForm" novalidate="true">
    <v-card>
      <v-card-title>
        <span class="headline">{{ editMode ? "Editarea" : "Adaugarea" }}</span>
      </v-card-title>
      <v-card-text>
        <v-container grid-list-md>
          <v-layout v-if="!loading" wrap>
            <v-flex xs12>
              <v-text-field
                :error-messages="errors.prefix"
                v-model="form.prefix"
                label="Prefixul (titlul universitar, etc)"
              ></v-text-field>
            </v-flex>
            <v-flex xs12>
              <v-text-field
                :error-messages="errors.name"
                v-model="form.name"
                label="Prenumele *"
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
                :error-messages="errors.surname"
                v-model="form.surname"
                label="Numele *"
              ></v-text-field>
            </v-flex>

            <v-flex xs12>
              <v-subheader>Tipul de persoana</v-subheader>
              <v-divider></v-divider>
            </v-flex>

            <v-btn-toggle v-model="form.staffType" mandatory>
              <v-btn
                :key="i"
                v-for="(staffType, i) in staffTypes"
                :value="staffType.id"
                rounded
              >
                {{ staffType.name }}
              </v-btn>
            </v-btn-toggle>

            <v-flex xs12>
              <v-subheader
                >Contul utilizatorului (in caz ca utilizatorul trebuie sa se
                logheze)</v-subheader
              >
              <v-divider></v-divider>
            </v-flex>
            <v-flex xs12>
              <v-text-field
                :error-messages="errors.user.email"
                v-model="form.user.email"
                label="Email"
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
import AxiosService from "@/AxiosService";
import { remapErrors } from "@/helpers/errorsHelper";
import LoadingPlaceholder from "@/components/common/LoadingPlaceholder.vue";
import fieldOfStudy from "@/constants/fieldOfStudyEnum";
import staffTypes, { staffTypesEnum } from "@/constants/staffTypes";

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
      staffTypes,
      form: {
        name: null,
        surname: null,
        patronymic: null,
        staffType: staffTypesEnum.secretary,
        user: {
          email: null,
        },
      },
      errors: {
        name: [],
        surname: [],
        patronymic: [],
        staffType: [],
        user: {
          email: [],
        },
      },
      semesters: [],
      facultyPersons: [],
      loading: false,
      loadingSave: false,
    };
  },
  mounted() {
    AxiosService.getInstance()
      .get("facultyPerson")
      .then((resp) => {
        this.facultyPersons = resp.items.map((f) => {
          f.name = `${f.surname} ${f.name}`;

          return f;
        });
      });

    if (this.editMode) {
      this.loading = true;

      AxiosService.getInstance()
        .get(`facultyPerson/${this.editId}`)
        .then((resp) => {
          this.loading = false;
          this.form = resp;
        });
    }
  },
  methods: {
    selectAllSpecialities() {
      this.form.semesters = this.semesters;
    },
    submitForm(e) {
      e.preventDefault();

      this.loadingSave = true;

      AxiosService.getInstance()
        .post("facultyPerson", this.form)
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
    getFieldOfStudy(type) {
      const foundFieldOfStudy = fieldOfStudy.find((f) => f.id === type);

      if (foundFieldOfStudy) {
        return foundFieldOfStudy.name;
      }

      return "";
    },
  },
};
</script>
