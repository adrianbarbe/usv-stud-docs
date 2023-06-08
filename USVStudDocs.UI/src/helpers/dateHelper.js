import moment from "moment";
import { vueInstance } from "@/main";
import dateFormats from "@/helpers/dateFormats";

export const formatShortDate = (value) => {
  if (!value) {
    return "";
  }
  // moment.locale(vueInstance.$i18n.locale);

  return moment.utc(value).format(dateFormats.short);
};

export const formatLongDate = (value) => {
  if (!value) {
    return "";
  }
  // moment.locale(vueInstance.$i18n.locale);

  return moment.utc(value).format(dateFormats.long);
};
