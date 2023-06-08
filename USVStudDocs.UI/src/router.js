import Vue from "vue";
import VueRouter from "vue-router";
import commonRoutes from "@/routes/commonRoutes";

Vue.use(VueRouter);

const router = new VueRouter({
  mode: "history",
  routes: [{ path: "*", redirect: "/notfound" }],
});

router.addRoutes(commonRoutes(router));

export default router;
