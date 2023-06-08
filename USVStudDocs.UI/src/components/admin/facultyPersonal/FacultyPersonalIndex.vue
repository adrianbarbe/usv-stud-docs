<template>
  <div>
    <faculty-personal-grid
      :items="items"
      :total="total"
      :loading="loading"
      @paginate="onPaginate"
      @editItem="editItem"
      @deleteItem="deleteItem"
    >
    </faculty-personal-grid>

    <v-dialog v-model="dialog" max-width="500px" scrollable>
      <add-edit-faculty-personal
        v-if="dialog"
        @shouldCloseAddEdit="dialog = false"
        @addedEdited="addedEdited"
        :edit-mode="editMode"
        :edit-id="editId"
      ></add-edit-faculty-personal>
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
import AddEditFacultyPersonal from "@/components/admin/facultyPersonal/AddEditFacultyPersonal.vue";
import FacultyPersonalGrid from "@/components/admin/facultyPersonal/FacultyPersonalGrid.vue";

export default {
  components: {
    AddEditFacultyPersonal,
    FacultyPersonalGrid,
    DeleteConfirm,
  },
  methods: {
    getItems(queryParams) {
      this.loading = true;

      AxiosService.getInstance()
        .get(`facultyPerson?${queryParams}`)
        .then((facultiesPersonal) => {
          this.items = facultiesPersonal.items;
          this.total = facultiesPersonal.total;
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
