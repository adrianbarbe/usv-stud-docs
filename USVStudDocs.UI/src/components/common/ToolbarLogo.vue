<template>
    <div :class="{'avatar-image': true, 'avatar-image-env': $config.configMode !== 'production'}">
        <img
            :src="require('../../assets/usv-logo-main.png')"
            alt="USV logo"
        >
        <div v-if="$config.configMode === 'development'" class="env-text-helper">Develop</div>
        <div v-if="$config.configMode === 'staging'" class="env-text-helper">Staging</div>
    </div>
</template>