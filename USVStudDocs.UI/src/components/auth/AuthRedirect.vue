<template>
  <v-progress-circular
      color="primary"
      indeterminate
      size="200"
  ></v-progress-circular>
</template>

<script>
import AxiosService from "@/AxiosService";
import {rolesEnum} from "@/constants/rolesEnum";
import store from "@/store";

export default {
  name: "AuthRedirect",
  mounted() {
    const queryParams = new URLSearchParams(window.location.search);

    const code = queryParams.get("code");

    AxiosService.getInstance({}, {}, true)
        .post(`oauth2/authorize`, {code: code})
        .then((res) => {
          this.$auth.login(res.id_token, res.refresh_token);

          const role = this.$auth.getRole();

          switch (role) {
            case rolesEnum.admin:
              this.$router.push({name: "adminDashboard"});
              break;
            case rolesEnum.secretary:
              this.$router.push({name: "secretaryDashboard"});
              break;
            case rolesEnum.student:
              this.$router.push({name: "studentDashboard"});
              break;
            case rolesEnum.analytic:
              break;
            default:
              break;
          }
        })
        .catch(() => {
          this.$store.commit("snackMessages/set", {
            message: "Contul Dvs nu este inregistrat in USV StudDocs. Va rugam sa va adresati la secretariatul facultatii Dvs. ",
            color: "error",
          });
          
          setTimeout(() => {
            this.$router.push({name: "sign-up"});
          }, 4000);
        });
  },
};
</script>

<style scoped></style>
