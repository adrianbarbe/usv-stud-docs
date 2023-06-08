<template>
  <v-card-text>
    <div class="title mb-2">Importul studentilor</div>
    <div class="mb-2">
      <v-alert outlined type="info" prominent border="left">
        <p>
          De aici puteti descarca template-uri CSV pentru completarea lor cu
          datele studentilor pentru importul ulterior.
        </p>
        <p>
          Versiunea cu numele, initiala tatalui si prenumele in coloane diferite
          -
          <v-btn color="primary" text small :href="`${apiEndpoint}/file/sample`"
            >Descarca</v-btn
          >
        </p>
        <p>
          Versiunea cu numele, initiala tatalui si prenumele concatenate
          impreuna intr-o coloana -
          <v-btn
            color="primary"
            text
            small
            :href="`${apiEndpoint}/file/sample/concatenated`"
            >Descarca</v-btn
          >
        </p>
      </v-alert>
      <v-alert border="bottom" colored-border type="warning" elevation="2">
        In cazul in care numele, initiala tatalui si prenumele sunt concatenate
        intr-un singur sir, este important ca initiala tatalui sa fie urmata de
        un punct ".", iar in cazul in care prenumele tatalui este dublu –
        initialele sa nu contina spatii (de exemplu, Alin Petru trebuie sa fie
        ca "A.P.").
      </v-alert>
      <div>
        In cazul in care numele, initiala tatalui si prenumele se afla intr-o
        singua coloana, va rugam sa selectati optiunea de mai jos
      </div>
      <v-switch
        v-model="studentNameConcatenated"
        label="Numele, initiala tatalui si prenumele sunt intr-o singura coloana"
      ></v-switch>
    </div>
    <div class="d-flex align-center">
      <v-file-input
        v-model="fileCodes"
        chips
        show-size
        filled
        accept="text/csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        label="CSV/XLSX fisierul cu studentii"
      ></v-file-input>
      <v-btn
        :disabled="!fileCodes || loading"
        :loading="loading"
        class="ml-6"
        color="primary"
        @click="uploadFile()"
        >Incarca CSV/XLSX
      </v-btn>
    </div>

    <v-alert v-if="resp.length > 0" type="error">
      <div v-for="stud in resp">{{ stud.surname }} {{ stud.name }}</div>
    </v-alert>
  </v-card-text>
</template>
<script>
import AxiosService from "@/AxiosService";
import { AppConfig } from "@/config/configFactory";
import config from "@/app.config";

let configuration = AppConfig(config);

export default {
  data: () => {
    return {
      fileCodes: null,
      resp: [],
      loading: false,
      studentNameConcatenated: false,
      apiEndpoint: configuration.apiEndpoint,
    };
  },
  mounted() {},
  methods: {
    uploadFile() {
      const formData = new FormData();
      formData.append("file", this.fileCodes);
      formData.append("studentNameConcatenated", this.studentNameConcatenated);

      let acessToken = localStorage.getItem("bearer_token");

      this.loading = true;

      AxiosService.getInstance({
        "Content-Type": "multipart/form-data",
        Authorization: `Bearer ${acessToken}`,
      })
        .post(`studentsImport/import/${this.$route.params.faculty}`, formData)
        .then((resp) => {
          this.resp = resp;

          this.fileCodes = null;
          this.loading = false;
        })
        .catch(() => {
          this.fileCodes = null;
          this.loading = false;
        });
    },
  },
};
</script>
