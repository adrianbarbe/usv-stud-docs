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
                :error-messages="errors.name"
                v-model="form.name"
                label="Numele"
              ></v-text-field>
            </v-flex>
            <v-flex xs12>
              <v-text-field
                :error-messages="errors.nameShort"
                v-model="form.nameShort"
                label="Acronimul"
              ></v-text-field>
            </v-flex>
            <v-flex xs12 md4>
              <v-text-field
                :error-messages="errors.orderBy"
                v-model="form.orderBy"
                v-mask="['##']"
                label="Ordinea"
              ></v-text-field>
            </v-flex>
            <v-flex xs12 md12>
              <v-select
                :items="faculties"
                :error-messages="errors.faculty"
                v-model="form.faculty"
                item-text="name"
                item-value="id"
                return-object
                label="Facultatea"
                chips
              ></v-select>
            </v-flex>

            <v-flex xs12 md12>
              <v-select
                  :items="secretaries"
                  :error-messages="errors.secretary"
                  v-model="form.secretary"
                  item-text="name"
                  item-value="id"
                  return-object
                  label="Secretara"
                  chips
              ></v-select>
            </v-flex>

            <v-flex xs12 md12>
              <v-select
                :items="semesters"
                :error-messages="errors.yearSemesters"
                v-model="form.yearSemesters"
                item-text="value"
                item-value="id"
                return-object
                label="Anul / Anii"
                multiple
                chips
                append-icon="control_point"
                @click:append="selectAllSpecialities"
              ></v-select>
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
        name: null,
        nameShort: null,
        orderBy: null,
        faculty: [],
        yearSemesters: [],
      },
      errors: {
        name: [],
        nameShort: [],
        orderBy: [],
        faculty: [],
        yearSemesters: [],
      },
      semesters: [],
      faculties: [],
      secretaries: [],
      loading: false,
      loadingSave: false,
    };
  },
  mounted() {
    AxiosService.getInstance()
      .get("faculty/getAll")
      .then((resp) => {
        this.faculties = resp;
      });

    AxiosService.getInstance()
        .get("facultyPerson/getSecretaries")
        .then((resp) => {
          this.secretaries = resp.map(r => {
            r.name = `${r.surname} ${r.name}`
            return r;
          });
        });

    AxiosService.getInstance()
      .get("semesters")
      .then((resp) => {
        this.semesters = resp.map((s) => {
          s.value = `Anul ${s.yearNumber} ${this.getFieldOfStudy(
            s.fieldOfStudy
          )}`;
          return s;
        });
      });

    if (this.editMode) {
      this.loading = true;

      AxiosService.getInstance()
        .get(`programStudy/${this.editId}`)
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
        .post("programStudy", this.form)
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
