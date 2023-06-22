<template>
  <div class="timeline-wrapper">
    <div class="timeline-item">
      <div class="animated-background">
        <div class="background-masker first"></div>
        <div class="background-masker second"></div>
        <div class="background-masker second-center"></div>
        <div class="background-masker third"></div>
        <div class="background-masker third-center"></div>
        <div class="background-masker fourth"></div>
        <div class="background-masker fifth"></div>
      </div>
    </div>
  </div>
</template>
<style scoped lang="scss">
.timeline-item {
  background: #fff;
  margin: 0 auto;
  max-width: 772px;
  min-height: 250px;
}

@keyframes placeHolderShimmer {
  0% {
    background-position: -468px 0;
  }
  100% {
    background-position: 468px 0;
  }
}

.animated-background {
  animation-duration: 1s;
  animation-fill-mode: forwards;
  animation-iteration-count: infinite;
  animation-name: placeHolderShimmer;
  animation-timing-function: linear;
  background: #f6f7f8;
  background: linear-gradient(to right, #eeeeee 8%, #dddddd 18%, #eeeeee 33%);
  background-size: 800px 104px;
  height: 246px;
  position: relative;
}

.background-masker {
  background: #fff;
  position: absolute;
}

.background-masker.first {
  top: 25px;
  left: 0;
  right: 0;
  height: 25px;
}

.background-masker.second {
  top: 75px;
  left: 0;
  right: 0;
  height: 25px;
}

.background-masker.second-center {
  top: 100px;
  left: 45%;
  width: 10%;
  height: 25px;
}

.background-masker.third {
  top: 125px;
  left: 0;
  right: 0;
  height: 25px;
}

.background-masker.third-center {
  top: 150px;
  left: 45%;
  width: 10%;
  height: 25px;
}

.background-masker.fourth {
  top: 175px;
  left: 0;
  right: 0;
  height: 25px;
}

.background-masker.fifth {
  top: 225px;
  left: 0;
  right: 0;
  height: 25px;
}
</style>
