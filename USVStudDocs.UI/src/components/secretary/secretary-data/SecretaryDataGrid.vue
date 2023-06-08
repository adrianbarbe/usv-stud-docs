<template>
  <v-data-table
    :headers="headers"
    :items="items"
    :server-items-length="total"
    :loading="loading"
    :options.sync="pagination"
    class="elevation-3"
    :footer-props="{
      itemsPerPageOptions: [10, 25, 50, 100, { text: 'Toate', value: -1 }],
    }"
  >
    <template v-slot:item.student="{ item }">
      {{ item.student.surname }} {{ item.student.patronymic }}
      {{ item.student.name }}
    </template>

    <template v-slot:item.secretary="{ item }">
      {{ item.secretary.surname }} {{ item.secretary.patronymic }}
      {{ item.secretary.name }}
    </template>

    <template v-slot:item.programStudy="{ item }">
      {{ item.student.programStudy.nameShort }}
    </template>

    <template v-slot:item.studyYear="{ item }">
      {{ item.student.yearSemester.yearNumber }}
    </template>

    <template v-slot:item.financialStatus="{ item }">
      {{ getFinancialStatus(item.student.financialStatus) }}
    </template>

    <template v-slot:item.fieldOfStudy="{ item }">
      {{ getFieldOfStudy(item.student.yearSemester.fieldOfStudy) }}
    </template>

    <template v-slot:item.certificateReason="{ item }">
      {{ item.certificateReason }}
    </template>

    <template v-slot:item.registrationDate="{ item }">
      {{ formatLongDate(item.registrationDate) }}
    </template>

    <template v-slot:item.certificateStatus="{ item }">
      <div class="text-center" style="min-width: 150px">
        <v-chip
          dark
          class="mx-2 ma-1"
          :color="getCertificateColor(item.certificateStatus)"
        >
          {{ getCertificateStatus(item.certificateStatus) }}
        </v-chip>
        <div class="mt-1" v-if="item.denyReason">
          <v-alert dense outlined type="error">
            {{ item.denyReason }}
          </v-alert>
        </div>
      </div>
    </template>

    <template v-slot:item.actions="{ item }">
      <v-btn
        v-if="status === certificateStatusEnum.new"
        large
        icon
        class="mx-0"
        @click="$emit('confirmItem', item.id)"
      >
        <v-icon color="primary">check_circle</v-icon>
      </v-btn>
      <v-btn
        v-if="status === certificateStatusEnum.new"
        large
        icon
        class="mx-0"
        @click="$emit('rejectItem', item.id)"
      >
        <v-icon color="red">cancel</v-icon>
      </v-btn>
      <v-btn
        v-if="status === certificateStatusEnum.approved"
        large
        icon
        class="mx-0"
        @click="$emit('previewPrintItem', item.id)"
      >
        <v-icon color="secondary">print</v-icon>
      </v-btn>
      <v-btn
        v-if="status === certificateStatusEnum.printed"
        large
        icon
        class="mx-0"
        @click="$emit('setSignedItem', item.id)"
      >
        <v-icon color="green">edit</v-icon>
      </v-btn>
    </template>
  </v-data-table>
</template>
<script>
import certificateStatus, {
  certificateStatusEnum,
} from "@/constants/certificateStatusEnum";
import financialStatus from "@/constants/financialStatusEnum";
import { formatLongDate } from "@/helpers/dateHelper";
import fieldOfStudy from "@/constants/fieldOfStudyEnum";

export default {
  props: {
    items: {
      type: Array,
    },
    total: {
      type: Number,
    },
    loading: {
      type: Boolean,
    },
    status: {
      type: Number,
    },
  },
  watch: {
    pagination(paginationVal, _) {
      this.$emit("paginate", paginationVal);
    },
  },
  data: () => ({
    certificateStatusEnum,

    pagination: {
      totalItems: 0,
    },
    headers: [
      { text: "Studentul", value: "student" },
      { text: "Domeniul de studii", value: "fieldOfStudy" },
      { text: "Programul de studii", value: "programStudy" },
      { text: "Statut financiar", value: "financialStatus" },
      { text: "Anul de inv.", value: "studyYear" },
      { text: "Data trimiterii", value: "registrationDate" },
      { text: "Numarul de inregistrare", value: "registrationNumber" },
      { text: "Motivul", value: "certificateReason" },
      { text: "Starea", value: "certificateStatus" },
      { text: "Secretarul responsabil", value: "secretary" },
      { text: "Actiuni", value: "actions" },
    ],
    formatLongDate,
  }),
  methods: {
    getCertificateStatus(type) {
      const foundCertificateStatus = certificateStatus.find(
        (f) => f.id === type
      );

      if (foundCertificateStatus) {
        return foundCertificateStatus.name;
      }

      return "";
    },

    getCertificateColor(type) {
      const foundCertificateStatus = certificateStatus.find(
        (f) => f.id === type
      );

      if (foundCertificateStatus) {
        return foundCertificateStatus.color;
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
