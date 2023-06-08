<template>
  <div>
    <students-grid
      :items="items"
      :total="total"
      :loading="loading"
      @editItem="editItem"
      @deleteItem="deleteItem"
      @deleteSelected="deleteSelectedDialog()"
      :selectedItems.sync="selectedItems"
    >
    </students-grid>

    <v-dialog v-model="dialog" max-width="450px" scrollable>
      <add-edit-student
        v-if="dialog"
        @shouldCloseAddEdit="dialog = false"
        @addedEdited="addedEdited"
        :edit-mode="editMode"
        :edit-id="editId"
        :items="items"
      ></add-edit-student>
    </v-dialog>

    <v-dialog v-model="dialogRemove" max-width="290">
      <delete-confirm
        @shouldCloseDeleteConfirm="dialogRemove = false"
        @removed="removed"
        :delete-id="deleteId"
      >
      </delete-confirm>
    </v-dialog>

    <v-dialog v-model="dialogDeleteSelected" persistent max-width="500">
      <v-card>
        <v-card-title class="subtitle-1"
          >Sunteti siguri ca doriti sa stergeti studentii
          selectati?</v-card-title
        >
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="primary" @click.native="dialogDeleteSelected = false"
            >Nu
          </v-btn>
          <v-btn color="primary" text @click.native="deleteSelected">Da </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-btn color="primary" dark fixed bottom right fab @click="addItem()">
      <v-icon>add</v-icon>
    </v-btn>
  </div>
</template>
<script>
import StudentsGrid from "./StudentsGrid";
import AddEditStudent from "./AddEditStudent";
import DeleteConfirm from "./DeleteConfirm";
import AxiosService from "@/AxiosService";

export default {
  components: {
    StudentsGrid,
    AddEditStudent,
    DeleteConfirm,
  },
  mounted() {
    this.getItemsWithRoute();
  },
  methods: {
    dismissDialogs() {
      this.dialogAddSeveral = false;

      this.selectedItems = [];
      this.items = [];
      this.total = 0;
    },
    getItemsWithRoute() {
      this.getItems(
        this.$route.params.faculty,
        this.$route.params.speciality,
        this.$route.params.semester
      );
    },
    deleteSelectedDialog() {
      this.dialogDeleteSelected = true;
    },
    deleteSelected() {
      this.loading = true;
      AxiosService.getInstance()
        .post(`student/deletes`, this.selectedItems)
        .then(() => {
          this.dialogDeleteSelected = false;
          this.loading = false;

          this.getItemsWithRoute();
        });
    },
    getItems(facultyId, specialityId, semesterId) {
      this.loading = true;

      AxiosService.getInstance()
        .get(`student/${facultyId}/${specialityId}/${semesterId}`)
        .then((students) => {
          this.items = students.items.map((item, index) => {
            item.index = index + 1;

            return item;
          });
          this.total = students.total;
          this.loading = false;
        });
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

      this.getItemsWithRoute();
    },
    deleteItem(itemId) {
      this.dialogRemove = true;
      this.deleteId = itemId;
    },
    removed() {
      this.dialogRemove = false;
      this.deleteId = null;

      this.getItemsWithRoute();
    },
  },
  watch: {
    "$route.params": function (newRoute, oldRoute) {
      if (
        newRoute.faculty !== oldRoute.faculty ||
        newRoute.speciality !== oldRoute.speciality ||
        newRoute.semester !== oldRoute.semester
      ) {
        this.getItems(newRoute.faculty, newRoute.speciality, newRoute.semester);
        this.selectedItems = [];
      }
    },
  },
  data: () => {
    return {
      dialog: false,
      dialogRemove: false,
      dialogDeleteSelected: false,
      dialogAddSeveral: false,

      editId: null,
      editMode: false,
      deleteId: null,
      items: [],
      total: null,

      loading: false,

      selectedItems: [],
    };
  },
};
</script>
