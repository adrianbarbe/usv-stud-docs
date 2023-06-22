export const objectQueryStringify = (params) => {
  let paramsRemapped = {
    sortDirection:
      params.sortDesc === null ? null : !params.sortDesc ? "asc" : "desc",
    sortBy: !params.sortBy ? "" : params.sortBy,
    pageNumber: params.page,
    itemsPerPage: params.itemsPerPage,
    filter:
      params.filter !== undefined
        ? JSON.stringify(params.filter)
        : JSON.stringify([]),
    searchString: params.searchString ? params.searchString : "",
  };

  return Object.keys(paramsRemapped)
    .map((key) => key + "=" + paramsRemapped[key])
    .join("&");
};

export const objectLogsQueryStringify = (params) => {
  let paramsRemapped = {
    objectId: params.objectId,
    itemsPerPage: params.itemsPerPage,
    direction: params.direction,
    filter:
      params.filter !== undefined
        ? JSON.stringify(params.filter)
        : JSON.stringify([]),
  };

  return Object.keys(paramsRemapped)
    .map((key) => key + "=" + paramsRemapped[key])
    .join("&");
};

export const arrayToQueryString = (array, key) => {
  return array
    .map((a) => {
      return `${key}=${encodeURIComponent(a)}`;
    })
    .join("&");
};
