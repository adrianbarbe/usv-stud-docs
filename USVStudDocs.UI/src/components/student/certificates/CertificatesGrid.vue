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
    <template v-slot:item.secretary="{ item }">
      {{ item.secretary.surname }} {{ item.secretary.patronymic }}
      {{ item.secretary.name }}
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
  </v-data-table>
</template>
<script>
import certificateStatus from "@/constants/certificateStatusEnum";
import { formatLongDate } from "@/helpers/dateHelper";

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
  },
  watch: {
    pagination(paginationVal, _) {
      this.$emit("paginate", paginationVal);
    },
  },
  data: () => ({
    pagination: {
      totalItems: 0,
    },
    headers: [
      { text: "Data trimiterii", value: "registrationDate" },
      { text: "Numarul de inregistrare", value: "registrationNumber" },
      { text: "Motivul", value: "certificateReason" },
      { text: "Starea", value: "certificateStatus" },
      { text: "Secretarul responsabil", value: "secretary" },
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
  },
};
</script>
