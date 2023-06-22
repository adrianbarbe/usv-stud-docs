<template>
  <div>
    <faculties-grid
      :items="items"
      :total="total"
      :loading="loading"
      @paginate="onPaginate"
      @editItem="editItem"
      @deleteItem="deleteItem"
    >
    </faculties-grid>

    <v-dialog v-model="dialog" max-width="500px" scrollable>
      <add-edit-faculty
        v-if="dialog"
        @shouldCloseAddEdit="dialog = false"
        @addedEdited="addedEdited"
        :edit-mode="editMode"
        :edit-id="editId"
      ></add-edit-faculty>
    </v-dialog>

    <v-dialog v-model="dialogRemove" max-width="290">
      <delete-confirm
        @shouldCloseDeleteConfirm="dialogRemove = false"
        @removed="removed"
        :delete-id="deleteId"
      >
      </delete-confirm>
    </v-dialog>

    <v-btn color="primary" dark fixed bottom right fab @click="addItem()">
      <v-icon>add</v-icon>
    </v-btn>
  </div>
</template>
<script>
import DeleteConfirm from "./DeleteConfirm";
import AxiosService from "@/AxiosService";
import { objectQueryStringify } from "@/helpers/querystringHelper";
import AddEditFaculty from "@/components/admin/faculties/AddEditFaculty.vue";
import FacultiesGrid from "@/components/admin/faculties/FacultiesGrid.vue";

export default {
  components: {
    AddEditFaculty,
    FacultiesGrid,
    DeleteConfirm,
  },
  methods: {
    getItems(queryParams) {
      this.loading = true;

      AxiosService.getInstance()
        .get(`faculty?${queryParams}`)
        .then((faculties) => {
          this.items = faculties.items;
          this.total = faculties.total;
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
    };
  },
};
</script>
