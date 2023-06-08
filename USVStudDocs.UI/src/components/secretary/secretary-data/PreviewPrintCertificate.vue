<template>
  <v-card>
    <v-card-title>
      <span class="headline">Previzualizare si listare</span>
    </v-card-title>
    <v-card-text>
      <v-container grid-list-md>
        <v-layout v-if="!loading" wrap>
          <v-flex xs12>
            <div v-if="certificate" id="print_wrapper">
              <div>
                <div class="text-title text-uppercase">
                  Universitatea "Ștefan cel Mare" din Suceava
                </div>
                <div class="text-title text-uppercase">
                  {{ certificate.certificateItem.student.faculty.name }}
                </div>
              </div>

              <div>
                <div class="text-title text-right">
                  Nr. {{ certificate.certificateItem.registrationNumber }}
                </div>
              </div>

              <div>
                <h1 class="certificate-title text-uppercase text-center">
                  Adeverință
                </h1>
              </div>

              <p>
                Studentul (a)
                <strong
                  >{{ certificate.certificateItem.student.surname }}
                  {{ certificate.certificateItem.student.patronymic }}
                  {{ certificate.certificateItem.student.name }}</strong
                >
                este înscris (ă) în anul universitar
                {{ certificate.studyYearStart }}/{{
                  certificate.studyYearStart + 1
                }}
                în anul
                {{
                  certificate.certificateItem.student.yearSemester.yearNumber
                }}
                de studii, program/domeniu de studii –
                {{ certificate.certificateItem.student.programStudy.name }} /
                {{
                  getFieldOfStudy(
                    certificate.certificateItem.student.yearSemester
                      .fieldOfStudy
                  )
                }}, forma de învățământ IF, regim:
                {{
                  getFinancialStatus(
                    certificate.certificateItem.student.financialStatus
                  )
                }}.
              </p>
              <p>
                Adeverința se eliberează pentru a-i servi la:
                {{ certificate.certificateItem.certificateReason }}.
              </p>
              <div class="signatures-container">
                <div>
                  <p>
                    <span class="text-uppercase"><strong>Decan,</strong></span>
                    <br />
                    {{ certificate.dean.prefix }} {{ certificate.dean.name }}
                    <span class="text-uppercase">{{
                      certificate.dean.surname
                    }}</span>
                  </p>
                </div>
                <div>
                  <p>
                    <span class="text-uppercase"
                      ><strong>Secretar șef, </strong></span
                    >
                    <br />
                    {{ certificate.secretaryHead.prefix }}
                    {{ certificate.secretaryHead.name }}
                    <span class="text-uppercase">{{
                      certificate.secretaryHead.surname
                    }}</span>
                  </p>
                </div>
                <div>
                  <p>
                    <span class="text-uppercase"
                      ><strong>Secretar, </strong></span
                    >
                    <br />
                    {{ certificate.secretary.prefix }}
                    {{ certificate.secretary.name }}
                    <span class="text-uppercase">{{
                      certificate.secretary.surname
                    }}</span>
                  </p>
                </div>
              </div>
            </div>
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
        @click.native="$emit('shouldClosePrint')"
        :disabled="loadingSave"
        >Refuza
      </v-btn>
      <v-btn color="secondary" @click="print" :disabled="loadingSave"
        >Listeaza
      </v-btn>
      <v-btn color="primary" @click="makePrint" :disabled="loadingSave"
        >Marcheaza ca listata
      </v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
import AxiosService from "@/AxiosService";
import { remapErrors } from "@/helpers/errorsHelper";
import LoadingPlaceholder from "@/components/common/LoadingPlaceholder.vue";
import fieldOfStudy from "@/constants/fieldOfStudyEnum";
import financialStatus from "@/constants/financialStatusEnum";

export default {
  props: {
    itemId: Number,
  },
  components: {
    LoadingPlaceholder,
  },
  mounted() {
    AxiosService.getInstance()
      .get(`certificateSecretary/getPrint/${this.itemId}`)
      .then((resp) => {
        this.loading = false;
        this.certificate = resp;
      });
  },
  data: () => {
    return {
      certificate: null,

      loading: false,
      loadingSave: false,
    };
  },
  methods: {
    getFieldOfStudy(type) {
      const foundFieldOfStudy = fieldOfStudy.find((f) => f.id === type);

      if (foundFieldOfStudy) {
        return foundFieldOfStudy.name;
      }

      return "";
    },
    getFinancialStatus(type) {
      const financialStatusFound = financialStatus.find((f) => f.id === type);

      if (financialStatusFound) {
        return financialStatusFound.name;
      }

      return "";
    },
    makePrint() {
      this.loadingSave = true;

      AxiosService.getInstance()
        .put(`certificateSecretary/confirmPrint/${this.itemId}`)
        .then((_) => {
          this.loadingSave = false;
          this.$emit("printed");
        })
        .catch((err) => {
          this.loadingSave = false;
        });
    },
    print() {
      const prtContent = document.getElementById("print_wrapper");
      const WinPrint = window.open(
        "",
        "",
        "left=0,top=0,width=800,height=900,toolbar=0,scrollbars=0,status=0"
      );

      WinPrint.document.write(`
            <style>
                body { font-family: "Roboto", sans-serif; } 
                h1 {font-size: 1.2em;} 
                h2 {font-size: 1em;}
                .text-uppercase {
                  text-transform: uppercase;
                }
                .text-center {
                  text-align: center;
                }
                .text-right {
                  text-align: right;
                }
                .text-title {
                  font-size: 1.1em;
                }
                .certificate-title {
                  margin-top: 1em;
                  margin-bottom: 1em;
                }
                .signatures-container {
                  display: flex;
                  justify-content: space-between;
                  margin-top: 3em;
                }
            </style>
      `);

      WinPrint.document.write(prtContent.innerHTML);
      WinPrint.document.close();
      WinPrint.focus();
      WinPrint.print();
      WinPrint.close();
    },
  },
};
</script>
<style scoped lang="scss">
h1 {
  font-size: 1.2em;
}
.text-uppercase {
  text-transform: uppercase;
}
.text-center {
  text-align: center;
}
.text-right {
  text-align: right;
}
.text-title {
  font-size: 1.1em;
}
.certificate-title {
  margin-top: 1em;
  margin-bottom: 1em;
}
.signatures-container {
  display: flex;
  justify-content: space-between;
  margin-top: 3em;
}
</style>
