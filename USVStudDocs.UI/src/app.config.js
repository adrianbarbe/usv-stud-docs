export default {
  default: {
    GA: { id: "" },
  },
  development: {
    apiEndpoint: "http://localhost:5000/api",
    appEndpoint: "http://localhost:8080",
    oauthClientId: "455359139560-h6e2hpgeheekl0po8ll0pv8blsf7fju9.apps.googleusercontent.com"
    // GA: {id: ""},
  },
  production: {
    apiEndpoint: "https://api.ulib.online/api",
    appEndpoint: "https://ulib.online",
    oauthClientId: "455359139560-h6e2hpgeheekl0po8ll0pv8blsf7fju9.apps.googleusercontent.com"
    // GA: {id: ""},
  },
};
