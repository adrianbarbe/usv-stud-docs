export default (to, from, next) => (router) => {
  if (!router.app.$auth.isAuthenticated()) {
    next({ name: "sign-up" });
  }
  next();
};
