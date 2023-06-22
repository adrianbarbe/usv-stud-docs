export const certificateStatusEnum = {
  new: 0,
  approved: 1,
  denied: 2,
  printed: 3,
  signed: 4,
};

export default [
  {
    id: certificateStatusEnum.new,
    name: "Nou",
    color: "#1E88E5",
  },
  {
    id: certificateStatusEnum.approved,
    name: "Aprobat",
    color: "#283593",
  },
  {
    id: certificateStatusEnum.denied,
    name: "Refuzat",
    color: "#E53935",
  },
  {
    id: certificateStatusEnum.printed,
    name: "Listat",
    color: "#26A69A",
  },
  {
    id: certificateStatusEnum.signed,
    name: "Semnat",
    color: "#2E7D32",
  },
];
