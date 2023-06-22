<template>
  <div
    :class="{
      'avatar-image-margin': true,
      'avatar-image-env': $config.configMode !== 'production',
    }"
  >
    <img
      width="85px"
      :src="require('../../assets/usv-logo-main.png')"
      alt="USV logo"
    />
    <div v-if="$config.configMode === 'development'" class="env-text-helper">
      Develop
    </div>
    <div v-if="$config.configMode === 'staging'" class="env-text-helper">
      Staging
    </div>
  </div>
</template>
<style lang="scss">
.avatar-image-margin {
  margin-top: 20px;
  position: relative;
}
</style>
