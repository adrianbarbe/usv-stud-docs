<template>
  <div>
    <div class="group-btns-wrapper">
      <v-btn
        small
        v-if="selectedItemsComputed.length"
        @click="$emit('deleteSelected')"
        color="red"
        dark
      >
        Sterge
      </v-btn>
    </div>
    <v-data-table
      :hide-default-footer="true"
      :headers="headers"
      :items="items"
      :server-items-length="total"
      :loading="loading"
      class="elevation-3"
      show-select
      v-model="selectedItemsComputed"
    >
      <template v-slot:item.surname="{ item }">
        <td class="text-xs-left">
          {{ item.surname }} {{ item.name }} {{ item.patronymic }}
        </td>
      </template>
      <!--            <template v-slot:item.birthday="{ item }">-->
      <!--                <td class="text-xl-start">{{formatShortDate(item.birthday)}}</td>-->
      <!--            </template>-->
      <!--            <template v-slot:item.gender="{ item }">-->
      <!--                <td class="text-xs-left">-->
      <!--                    <div v-if="item.gender === 0">Masculin</div>-->
      <!--                    <div v-if="item.gender === 1">Feminin</div>-->
      <!--                </td>-->
      <!--            </template>-->
      <template v-slot:item.actions="{ item }">
        <v-btn icon class="mx-0" @click="$emit('editItem', item.id)">
          <v-icon color="primary">edit</v-icon>
        </v-btn>
        <v-btn icon class="mx-0" @click="$emit('deleteItem', item.id)">
          <v-icon color="red">delete</v-icon>
        </v-btn>
      </template>
    </v-data-table>
  </div>
</template>
<script>
import { formatShortDate } from "@/helpers/dateHelper";

export default {
  props: {
    items: {
      type: Array,
    },
    selectedItems: {
      type: Array,
    },
    total: {
      type: Number,
    },
    loading: {
      type: Boolean,
    },
  },
  computed: {
    selectedItemsComputed: {
      get() {
        return this.selectedItems;
      },
      set(val) {
        this.$emit("update:selectedItems", val);
      },
    },
  },
  data: () => ({
    formatShortDate,
    selected: [],
    headers: [
      { text: "#", value: "index", sortable: false },
      { text: "Numele, Prenumele", value: "surname", sortable: false },
      // {text: 'Data de nastere', value: 'birthday', sortable: false},
      // {text: 'Sexul', value: 'gender', sortable: false},
      { text: "Actiuni", value: "actions", sortable: false },
    ],
  }),
};
</script>

<style lang="scss">
.group-btns-wrapper {
  margin-bottom: 10px;
  height: 36px;

  & .v-btn {
    margin-right: 10px;
  }
}
</style>
