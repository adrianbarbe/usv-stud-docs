<template>
  <div>
    <div v-if="!loading">
      <template v-if="items.length === 0">
        <div class="body-2 text-center mt-3">Nu e gasit</div>
      </template>
      <template v-for="(item, index) in items">
        <v-list-item
          class="list-title"
          active-class="list-title-active"
          exact-active-class="list-title-active"
          ripple
          v-if="item && !item.children && !item.divider"
          :key="index"
          @click="$emit('selectValue', item)"
          :to="item.route"
        >
          <v-list-item-action v-if="item.icon">
            <v-icon>{{ item.icon }}</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>
              {{ item.name }}
            </v-list-item-title>
          </v-list-item-content>
        </v-list-item>

        <v-list-group
          no-action
          v-else-if="item && item.children"
          v-model="item.model"
          :key="item.name"
          :prepend-icon="item.icon"
          class="v-list-group"
        >
          <!--                    <v-list-item class="v-list-tile">-->
          <template v-slot:activator>
            <v-list-item-content>
              <v-list-item-title class="tile-title-parent">
                {{ item.name }}
              </v-list-item-title>
            </v-list-item-content>
          </template>
          <!--                    </v-list-item>-->

          <v-list-item
            ripple
            v-if="child && !child.children"
            v-for="(child, index) in item.children"
            :key="index"
            class="list-title"
            exact-active-class="list-title-active"
            active-class="list-title-active"
            @click="$emit('selectValue', child)"
            :to="child.route"
          >
            <v-list-item-action v-if="child.icon">
              <v-icon>{{ child.icon }}</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>
                {{ child.name }}
              </v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-list-item
            class="list-title-not-found"
            v-if="
              item &&
              (item.children === undefined ||
                (item.children !== undefined && item.children.length === 0))
            "
          >
            <v-list-item-content>
              <v-list-item-title>
                <i>Nu e gasit</i>
              </v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-list-group
            v-if="child && child.children"
            v-for="(child, index) in item.children"
            :key="index"
            sub-group
            no-action
            v-model="child.model"
          >
            <!--                        <v-list-item exact-active-class="list-title-active">-->
            <template v-slot:activator>
              <v-list-item-content>
                <v-list-item-title class="tile-title-parent">
                  {{ child.name }}
                </v-list-item-title>
              </v-list-item-content>
            </template>
            <!--                        </v-list-item>-->

            <v-list-item
              ripple
              v-if="child && child.children"
              v-for="(childNested, index) in child.children"
              :key="index"
              class="list-title"
              exact-active-class="list-title-active"
              active-class="list-title-active"
              @click="$emit('selectValue', childNested)"
              :to="childNested.route"
            >
              <v-list-item-content>
                <v-list-item-title>
                  {{ childNested.name }}
                </v-list-item-title>
              </v-list-item-content>
            </v-list-item>

            <v-list-item
              class="list-title-not-found"
              v-if="
                child &&
                (child.children === undefined ||
                  (child.children !== undefined && child.children.length === 0))
              "
            >
              <v-list-item-content>
                <v-list-item-title>
                  <i>Nu e gasit</i>
                </v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </v-list-group>
        </v-list-group>
        <v-divider v-else-if="item && item.divider" dark></v-divider>
      </template>
    </div>
    <div v-if="loading">
      <v-container
        align-center
        align-content-center
        grid-list-md
        fluid
        fill-height
      >
        <v-layout row wrap justi>
          <v-progress-circular
            :size="150"
            color="primary"
            indeterminate
          ></v-progress-circular>
        </v-layout>
      </v-container>
    </div>
  </div>
</template>
<script>
export default {
  props: {
    items: {
      type: Array,
    },
    withNavigation: {
      type: Boolean,
      default: true,
    },
    loading: {
      type: Boolean,
      default: false,
    },
  },
};
</script>
<style>
.list-title-active {
  background: rgba(0, 0, 0, 0.12);
}

.list-title-not-found {
  color: #ccc;
}
</style>
