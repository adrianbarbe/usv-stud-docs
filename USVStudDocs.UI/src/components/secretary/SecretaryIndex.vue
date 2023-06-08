<template>
  <div class="d-flex align-center flex-column">
    <v-card class="container">
      <v-tabs
        v-model="tab"
        background-color="primary"
        centered
        dark
        fixed-tabs
        icons-and-text
      >
        <v-tabs-slider></v-tabs-slider>

        <v-tab href="#tab-1">
          <v-badge color="red" :content="countData.new"> Noi </v-badge>
          <v-icon>mdi-book-plus</v-icon>
        </v-tab>

        <v-tab href="#tab-2">
          <v-badge color="green" :content="countData.approved">
            Aprobate
          </v-badge>
          <v-icon>mdi-check-all</v-icon>
        </v-tab>

        <v-tab href="#tab-3">
          <v-badge color="blue" :content="countData.printed"> Listate </v-badge>
          <v-icon>mdi-printer</v-icon>
        </v-tab>

        <v-tab href="#tab-4">
          Refuzate
          <v-icon>mdi-close</v-icon>
        </v-tab>

        <v-tab href="#tab-5">
          Arhiva (semnate)
          <v-icon>mdi-file-document-multiple-outline</v-icon>
        </v-tab>
      </v-tabs>

      <v-tabs-items v-model="tab">
        <v-tab-item key="1" value="tab-1" class="pb-5">
          <secretary-data-index
            :status="certificateStatusEnum.new"
            @updated="getCount"
            v-if="tab === 'tab-1'"
          ></secretary-data-index>
        </v-tab-item>

        <v-tab-item key="2" value="tab-2" class="pb-5">
          <secretary-data-index
            :status="certificateStatusEnum.approved"
            @updated="getCount"
            v-if="tab === 'tab-2'"
          ></secretary-data-index>
        </v-tab-item>

        <v-tab-item key="3" value="tab-3" class="pb-5">
          <secretary-data-index
            :status="certificateStatusEnum.printed"
            @updated="getCount"
            v-if="tab === 'tab-3'"
          ></secretary-data-index>
        </v-tab-item>

        <v-tab-item key="4" value="tab-4" class="pb-5">
          <secretary-data-index
            :status="certificateStatusEnum.denied"
            @updated="getCount"
            v-if="tab === 'tab-4'"
          ></secretary-data-index>
        </v-tab-item>

        <v-tab-item key="5" value="tab-5" class="pb-5">
          <secretary-data-index
            :status="certificateStatusEnum.signed"
            @updated="getCount"
            v-if="tab === 'tab-5'"
          ></secretary-data-index>
        </v-tab-item>
      </v-tabs-items>
    </v-card>

    <v-dialog
      v-model="noCommonNumberDialog"
      persistent
      max-width="500px"
      scrollable
    >
      <common-number-introduction
        v-if="noCommonNumberDialog"
        @commonNumberAdded="commonNumberAdded"
      ></common-number-introduction>
    </v-dialog>
  </div>
</template>
<script>
import SecretaryDataIndex from "@/components/secretary/secretary-data/SecretaryDataIndex.vue";
import AxiosService from "@/AxiosService";
import { certificateStatusEnum } from "@/constants/certificateStatusEnum";
import CommonNumberIntroduction from "@/components/secretary/CommonNumberIntroduction.vue";

export default {
  components: {
    SecretaryDataIndex,
    CommonNumberIntroduction,
  },
  mounted() {
    this.getCommonNumber();
    this.getCount();

    setInterval(() => {
      this.getCount();
    }, 30000);
  },
  methods: {
    getCommonNumber() {
      AxiosService.getInstance()
        .get(`secretaryCommonNumber/getToday`)
        .then((commonNumber) => {
          if (!commonNumber.number) {
            this.noCommonNumberDialog = true;
          }
        });
    },
    commonNumberAdded() {
      this.noCommonNumberDialog = false;
    },
    getCount() {
      AxiosService.getInstance()
        .get(`certificateSecretary/count`)
        .then((countData) => {
          this.countData = countData;
        });
    },
  },
  data: () => {
    return {
      certificateStatusEnum,
      noCommonNumberDialog: false,
      countData: {
        new: 0,
        approved: 0,
        printed: 0,
      },
      tab: null,
    };
  },
};
</script>
