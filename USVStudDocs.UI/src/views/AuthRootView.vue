<template>
    <v-app>
            <div class="d-flex fill-height align-center justify-center flex-column">
                <router-view/>
            </div>
        <snack-displayer/>
    </v-app>
</template>

<script>
import SnackDisplayer from "@/components/common/SnackDisplayer";

export default {
    components: {
        snackDisplayer: SnackDisplayer,
    },
    data: () => ({
        drawerApp: true,
    }),
}
</script>
