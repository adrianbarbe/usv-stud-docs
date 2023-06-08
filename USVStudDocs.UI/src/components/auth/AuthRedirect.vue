<template>
  <v-progress-circular
    color="primary"
    indeterminate
    size="200"
  ></v-progress-circular>
</template>

<script>
import AxiosService from "@/AxiosService";
import { rolesEnum } from "@/constants/rolesEnum";

export default {
  name: "AuthRedirect",
  mounted() {
    const queryParams = new URLSearchParams(window.location.search);

    const code = queryParams.get("code");

    AxiosService.getInstance({}, {}, true)
      .post(`oauth2/authorize`, { code: code })
      .then((res) => {
        this.$auth.login(res.id_token, res.refresh_token);

        const role = this.$auth.getRole();

        switch (role) {
          case rolesEnum.admin:
            this.$router.push({ name: "adminDashboard" });
            break;
          case rolesEnum.secretary:
            this.$router.push({ name: "secretaryDashboard" });
            break;
          case rolesEnum.student:
            this.$router.push({ name: "studentDashboard" });
            break;
          case rolesEnum.analytic:
            break;
          default:
            break;
        }
      });
  },
};
</script>

<style scoped></style>
