<template>
  <div class="d-flex align-center flex-column">
    <div class="mb-10" v-if="studentInfo">
      <table>
        <tr>
          <td>
            <div class="text-right">
              <strong>Numele, IT, Prenumele: </strong>
            </div>
          </td>
          <td>
            {{ studentInfo.surname }} {{ studentInfo.patronymic }}
            {{ studentInfo.name }}
          </td>
        </tr>
        <tr>
          <td>
            <div class="text-right">
              <strong class="text-right">Facultatea: </strong>
            </div>
          </td>
          <td>{{ studentInfo.faculty.nameShort }}</td>
        </tr>
        <tr>
          <td>
            <div class="text-right">
              <strong class="text-right">Domeniu de studii: </strong>
            </div>
          </td>
          <td>{{ getFieldOfStudy(studentInfo.yearSemester.fieldOfStudy) }}</td>
        </tr>
        <tr>
          <td>
            <div class="text-right">
              <strong class="text-right">Program de studii: </strong>
            </div>
          </td>
          <td>{{ studentInfo.programStudy.nameShort }}</td>
        </tr>
        <tr>
          <td>
            <div class="text-right">
              <strong class="text-right">An studii: </strong>
            </div>
          </td>
          <td>{{ studentInfo.yearSemester.yearNumber }}</td>
        </tr>
        <tr>
          <td>
            <div class="text-right">
              <strong class="text-right">Statut financiar: </strong>
            </div>
          </td>
          <td>{{ getFinancialStatus(studentInfo.financialStatus) }}</td>
        </tr>
      </table>
    </div>

    <v-btn color="primary" x-large @click="addItem()"
      >Creaza noua cerere pentru adeverinta</v-btn
    >
    <certificates-grid
      class="mx-10 mt-10"
      :items="items"
      :total="total"
      :loading="loading"
      @paginate="onPaginate"
      @editItem="editItem"
      @deleteItem="deleteItem"
    >
    </certificates-grid>

    <v-dialog v-model="dialog" max-width="800px" scrollable>
      <add-certificate
        v-if="dialog"
        @shouldCloseAddEdit="dialog = false"
        @addedEdited="addedEdited"
        :edit-mode="editMode"
        :edit-id="editId"
      ></add-certificate>
    </v-dialog>
  </div>
</template>
<script>
import AxiosService from "@/AxiosService";
import { objectQueryStringify } from "@/helpers/querystringHelper";
import AddCertificate from "@/components/student/certificates/AddCertificate.vue";
import CertificatesGrid from "@/components/student/certificates/CertificatesGrid.vue";
import fieldOfStudy from "@/constants/fieldOfStudyEnum";
import financialStatusEnum from "@/constants/financialStatusEnum";

export default {
  components: {
    AddCertificate,
    CertificatesGrid,
  },
  mounted() {
    AxiosService.getInstance()
      .get(`certificateStudent/me`)
      .then((student) => {
        this.studentInfo = student;
      });
  },
  methods: {
    getItems(queryParams) {
      this.loading = true;

      AxiosService.getInstance()
        .get(`certificateStudent?${queryParams}`)
        .then((certificates) => {
          this.items = certificates.items;
          this.total = certificates.total;
          this.loading = false;
        });
    },
    onPaginate(paginationData) {
      if (
        this.items.length === 0 ||
        JSON.stringify(this.pagination) !== JSON.stringify(paginationData)
      ) {
        this.getItems(objectQueryStringify(paginationData));
      }

      this.pagination = paginationData;
    },
    addItem() {
      this.dialog = true;
      this.editId = null;
      this.editMode = false;
    },
    editItem(itemId) {
      this.dialog = true;
      this.editId = itemId;
      this.editMode = true;
    },
    addedEdited() {
      this.dialog = false;
      this.editId = null;

      this.getItems(objectQueryStringify(this.pagination));
    },
    deleteItem(itemId) {
      this.dialogRemove = true;
      this.deleteId = itemId;
    },
    removed() {
      this.dialogRemove = false;
      this.deleteId = null;

      this.getItems(objectQueryStringify(this.pagination));
    },

    getFieldOfStudy(type) {
      const foundFieldOfStudy = fieldOfStudy.find((f) => f.id === type);

      if (foundFieldOfStudy) {
        return foundFieldOfStudy.name;
      }

      return "";
    },
    getFinancialStatus(type) {
      const financialStatus = financialStatusEnum.find((f) => f.id === type);

      if (financialStatus) {
        return financialStatus.name;
      }

      return "";
    },
  },
  data: () => {
    return {
      dialog: false,
      dialogRemove: false,
      editId: null,
      editMode: false,
      deleteId: null,
      items: [],
      total: null,
      loading: false,
      pagination: {
        totalItems: 0,
      },

      studentInfo: null,
    };
  },
};
</script>
<style lang="scss" scoped>
table {
  td {
    padding: 0 10px;
  }
}
</style>
