import routeGuard from "../routeGuards/routeGuard";
import AdminRootView from "@/views/AdminRootView.vue";
import AuthRootView from "@/views/AuthRootView.vue";
import SignUp from "@/components/auth/SignUp.vue";
import AuthRedirect from "@/components/auth/AuthRedirect.vue";
import NotFoundView from "@/views/NotFoundView";
import TermsAndConditions from "@/components/auth/TermsAndConditions.vue";
import ChooseMenuItem from "@/components/common/ChooseMenuItem.vue";
import ProgramStudyView from "@/views/admin/ProgramStudyView.vue";
import AdminSettingsView from "@/views/admin/AdminSettingsView.vue";
import FacultiesView from "@/views/admin/FacultiesView.vue";
import FacultyPersonalView from "@/views/admin/FacultyPersonalView.vue";
import StudentsView from "@/views/admin/StudentsView.vue";
import StudentRootView from "@/views/StudentRootView.vue";
import CertificatesIndex from "@/components/student/certificates/CertificatesIndex.vue";
import SecretaryIndex from "@/components/secretary/SecretaryIndex.vue";

export default (router) => {
  return [
    { path: "/", redirect: "/auth/sign-up" },
    {
      path: "/admin",
      name: "adminRoot",
      component: AdminRootView,
      beforeEnter: (to, from, next) => routeGuard(to, from, next)(router),
      children: [
        {
          path: "",
          name: "adminDashboard",
          component: ChooseMenuItem,
        },
        {
          path: "settings",
          name: "adminSettings",
          component: AdminSettingsView,
        },
        {
          path: "studentsList/:faculty?/import",
          name: "studentsListImport",
          component: StudentsView,
        },
        {
          path: "studentsList/:faculty?/:speciality?/:semester?",
          name: "studentsList",
          component: StudentsView,
        },
        {
          path: "programStudy",
          name: "programStudy",
          component: ProgramStudyView,
        },
        {
          path: "faculties",
          name: "faculties",
          component: FacultiesView,
        },
        {
          path: "facultyPersonal",
          name: "facultyPersonal",
          component: FacultyPersonalView,
        },
      ],
    },
    {
      path: "/student",
      name: "studentRoot",
      component: StudentRootView,
      beforeEnter: (to, from, next) => routeGuard(to, from, next)(router),
      children: [
        {
          path: "",
          name: "studentDashboard",
          component: CertificatesIndex,
        },
      ],
    },
    {
      path: "/secretary",
      name: "secretaryRoot",
      component: StudentRootView,
      beforeEnter: (to, from, next) => routeGuard(to, from, next)(router),
      children: [
        {
          path: "",
          name: "secretaryDashboard",
          component: SecretaryIndex,
        },
      ],
    },
    {
      path: "/auth",
      component: AuthRootView,
      children: [
        {
          path: "terms-and-conditions",
          name: "terms-and-conditions",
          component: TermsAndConditions,
        },
        {
          path: "sign-up",
          name: "sign-up",
          component: SignUp,
        },
        {
          path: "redirect",
          name: "redirect",
          component: AuthRedirect,
        },
      ],
    },
    { path: "/404", component: NotFoundView },
    { path: "/:pathMatch(.*)*", redirect: "/404" },
  ];
};
