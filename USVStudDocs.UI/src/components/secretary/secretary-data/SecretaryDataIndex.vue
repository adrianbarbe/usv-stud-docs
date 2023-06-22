<template>
  <div class="d-flex align-center flex-column">
    <secretary-data-grid
      class="mx-10 mt-10"
      :items="items"
      :total="total"
      :loading="loading"
      :status="status"
      @paginate="onPaginate"
      @confirmItem="confirmItem"
      @rejectItem="rejectItem"
      @previewPrintItem="previewPrint"
      @setSignedItem="setSignedItem"
    >
    </secretary-data-grid>

    <v-dialog v-model="dialogReject" max-width="800px" scrollable>
      <reject-request-certificate
        v-if="dialogReject"
        @shouldCloseRefuse="dialogReject = false"
        @rejected="rejected"
        :item-id="rejectId"
      ></reject-request-certificate>
    </v-dialog>

    <v-dialog v-model="dialogConfirm" max-width="500px" scrollable>
      <confirm-certificate
        v-if="dialogConfirm"
        @shouldCloseRefuze="dialogConfirm = false"
        @confirmed="confirmed"
        :item-id="confirmId"
      ></confirm-certificate>
    </v-dialog>

    <v-dialog v-model="dialogPrint" max-width="800px" scrollable>
      <preview-print-certificate
        v-if="dialogPrint"
        @shouldClosePrint="dialogPrint = false"
        @printed="printed"
        :item-id="printId"
      ></preview-print-certificate>
    </v-dialog>
  </div>
</template>
<script>
import AxiosService from "@/AxiosService";
import { objectQueryStringify } from "@/helpers/querystringHelper";
import SecretaryDataGrid from "@/components/secretary/secretary-data/SecretaryDataGrid.vue";
import RejectRequestCertificate from "@/components/secretary/secretary-data/RejectRequestCertificate.vue";
import ConfirmCertificate from "@/components/secretary/secretary-data/ConfirmCertificate.vue";
import { certificateStatusEnum } from "@/constants/certificateStatusEnum";
import PreviewPrintCertificate from "@/components/secretary/secretary-data/PreviewPrintCertificate.vue";

export default {
  components: {
    PreviewPrintCertificate,
    RejectRequestCertificate,
    ConfirmCertificate,
    SecretaryDataGrid,
  },
  props: {
    status: {
      type: Number,
    },
  },
  methods: {
    getItems(queryParams) {
      this.loading = true;

      AxiosService.getInstance()
        .get(`certificateSecretary/${this.status}?${queryParams}`)
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
    rejectItem(itemId) {
      this.dialogReject = true;
      this.rejectId = itemId;
    },
    rejected() {
      this.dialogReject = false;
      this.rejectId = null;

      this.getItems(objectQueryStringify(this.pagination));

      this.$emit("updated");
    },
    confirmItem(itemId) {
      this.dialogConfirm = true;
      this.confirmId = itemId;
    },
    confirmed() {
      this.dialogConfirm = false;
      this.confirmId = null;

      this.getItems(objectQueryStringify(this.pagination));

      this.$emit("updated");
    },

    previewPrint(itemId) {
      this.dialogPrint = true;
      this.printId = itemId;
    },
    printed() {
      this.dialogPrint = false;
      this.printId = null;

      this.getItems(objectQueryStringify(this.pagination));

      this.$emit("updated");
    },
    setSignedItem(itemId) {
      AxiosService.getInstance()
        .put(`certificateSecretary/confirmSignature/${itemId}`)
        .then((_) => {
          this.getItems(objectQueryStringify(this.pagination));
        })
        .catch((err) => {});
    },
  },
  data: () => {
    return {
      dialogReject: false,
      dialogConfirm: false,
      rejectId: null,
      confirmId: null,

      dialogPrint: false,
      printId: null,

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
