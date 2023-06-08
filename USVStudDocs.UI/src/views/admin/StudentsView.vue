<template>
  <v-container grid-list-md fluid fill-height>
    <v-layout row wrap style="width: 100%">
      <v-flex style="height: calc(100vh - 88px); position: fixed; width: 300px">
        <v-card height="100%">
          <v-navigation-drawer :mobile-breakpoint="0" :width="300">
            <v-list dense>
              <template v-for="studNav in studNavigationItems">
                <v-subheader
                  class="text-subtitle-1 d-block mt-5 mb-7 grey--text text--darken-1"
                  >{{ studNav.name }}
                  <v-btn
                    x-small
                    :to="{
                      name: 'studentsListImport',
                      params: { faculty: studNav.id },
                    }"
                    >Import</v-btn
                  >
                </v-subheader>
                <navigation-list-template :items="studNav.programStudies" />
              </template>
            </v-list>
          </v-navigation-drawer>
        </v-card>
      </v-flex>

      <v-flex style="margin-left: 300px; width: calc(100% - 460px)">
        <v-card>
          <v-container
            v-if="!isSelectedSemester && $route.name !== 'studentsListImport'"
            grid-list-md
          >
            <v-layout wrap>
              <v-flex xs12>
                <div class="headline">
                  <v-icon>keyboard_backspace</v-icon>
                  Selectati facultatea, prgramul de studii si anul
                </div>
              </v-flex>
            </v-layout>
          </v-container>
          <students-index
            v-if="isSelectedSemester && $route.name === 'studentsList'"
          />
          <students-import-index v-if="$route.name === 'studentsListImport'" />
        </v-card>
      </v-flex>
    </v-layout>
  </v-container>
</template>
<script>
import AxiosService from "@/AxiosService";
import NavigationListTemplate from "@/components/common/NavigationListTemplate.vue";
import StudentsIndex from "@/components/admin/students/StudentsIndex.vue";
import StudentsImportIndex from "@/components/admin/studentsImport/StudentsImportIndex.vue";

export default {
  components: {
    NavigationListTemplate,
    StudentsIndex,
    StudentsImportIndex,
  },
  data: () => {
    return {
      studNavigationItems: [],
      isSelectedSemester: false,
      loading: false,
    };
  },
  props: {
    routeTo: {
      type: String,
      default: "studentsList",
    },
  },
  mounted() {
    this.isSelectedSemester = this.$route.params.semester !== undefined;

    this.loading = true;

    AxiosService.getInstance()
      .get("navigation")
      .then((navItems) => {
        this.loading = false;
        this.studNavigationItems = this.remapStudNavItems(navItems);
      });
  },
  watch: {
    $route: function (route, prevRoute) {
      if (route.params.semester !== prevRoute.params.semester) {
        this.isSelectedSemester = route.params.semester !== undefined;
      }
    },
  },
  methods: {
    remapStudNavItems(studNavigationItems) {
      if (studNavigationItems === undefined) {
        return [];
      }

      let studNavigationItemsRemapped = studNavigationItems.slice(0);

      studNavigationItemsRemapped.map((fac) => {
        fac.programStudies.map((spec) => {
          spec.children = spec.yearSemesters.map((sem) => {
            sem.name = `Anul ${sem.yearNumber}`;
            sem.route = {
              name: this.routeTo,
              params: {
                faculty: fac.id,
                speciality: spec.id,
                semester: sem.id,
              },
            };

            return sem;
          });

          let someSemesterActive = spec.children.find((ch) => ch.model);

          if (
            spec.id.toString() === this.$route.params.speciality &&
            someSemesterActive !== undefined
          ) {
            spec.model = true;
          }

          delete spec.semesters;

          return spec;
        });

        return fac;
      });

      return studNavigationItemsRemapped;
    },
  },
};
</script>
