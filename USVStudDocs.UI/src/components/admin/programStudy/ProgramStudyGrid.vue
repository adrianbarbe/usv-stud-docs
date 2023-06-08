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
    <template v-slot:item.faculty="{ item }">
      {{ item.faculty.name }}
    </template>

    <template v-slot:item.actions="{ item }">
      <v-btn icon class="mx-0" @click="$emit('editItem', item.id)">
        <v-icon color="primary">edit</v-icon>
      </v-btn>
      <v-btn icon class="mx-0" @click="$emit('deleteItem', item.id)">
        <v-icon color="red">delete</v-icon>
      </v-btn>
    </template>
  </v-data-table>
</template>
<script>
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
      { text: "Numele", value: "name" },
      { text: "Acronimul", value: "nameShort" },
      { text: "Ordinea", value: "orderBy" },
      { text: "Facultatea", value: "faculty" },
      { text: "Actiuni", value: "actions", sortable: false },
    ],
  }),
};
</script>
