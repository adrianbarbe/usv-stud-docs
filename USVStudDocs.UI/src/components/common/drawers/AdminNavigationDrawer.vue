<template>
  <v-navigation-drawer
    fixed
    app
    :clipped="$vuetify.breakpoint.mdAndUp"
    :value="drawer"
    width="260"
    @input="$emit('toggleDrawer', $event)"
  >
    <v-list dense>
      <v-subheader class="mt-4 subtitle-1 grey--name name--darken-1"
        >USV Stud Docs</v-subheader
      >
      <navigation-list-template
        :items="mainDashboardItems"
        @selectValue="setTopNavigationItem($event)"
      />
    </v-list>
  </v-navigation-drawer>
</template>
<script>
import { mapMutations } from "vuex";
import NavigationListTemplate from "@/components/common/NavigationListTemplate.vue";

export default {
  props: {
    drawer: {
      type: Boolean,
    },
  },
  components: {
    "navigation-list-template": NavigationListTemplate,
  },
  mounted() {
    this.mainDashboardItems = this.remapItems(this.mainDashboardItemsRaw);
  },
  methods: {
    ...mapMutations(["setTopNavigationItem"]),
    remapItems(items) {
      let itemsRemapped = items.slice(0);

      return itemsRemapped.map((item) => {
        if (item.children !== undefined) {
          item.children = item.children.map((ch) => {
            if (
              ch.route &&
              ch.route.name &&
              this.$route.name === ch.route.name
            ) {
              ch.model = true;
              this.setTopNavigationItem(ch);
            }
            return ch;
          });

          let someActiveChildren = item.children.find((ch) => ch.model);

          if (someActiveChildren !== undefined) {
            item.model = true;
          }

          return item;
        }

        if (
          item.route &&
          item.route.name &&
          this.$route.name === item.route.name
        ) {
          this.setTopNavigationItem(item);
        }

        return item;
      });
    },
  },
  data: () => ({
    mainDashboardItems: [],
    mainDashboardItemsRaw: [
      {
        icon: "settings",
        name: "Setari de baza",
        route: { name: "adminSettings" },
      },
      {
        icon: "list",
        name: "Lista studentilor",
        route: { name: "studentsList" },
      },
      {
        icon: "local_library",
        name: "Programele de studii",
        route: { name: "programStudy" },
      },
      {
        icon: "history_edu",
        name: "Facultatile",
        route: { name: "faculties" },
      },
      {
        icon: "manage_accounts",
        name: "Personalul facultatilor",
        route: { name: "facultyPersonal" },
      },
    ],
  }),
};
</script>
