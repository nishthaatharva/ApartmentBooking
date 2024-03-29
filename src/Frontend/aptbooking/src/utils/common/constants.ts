export const dataTableProps = {
  PAGE_SIZES: [10, 20, 30, 50],
};

export const StorageKey = {
  token: "token",
  userInfo: "userInfo",
};

export const Regex = {
  noSpaceValidationPattern: /^\S*$/,
  passwordValidationPattern: /^(?=.*[A-Z])(?=.*[a-z])(?=.*[^a-zA-Z0-9]).{8,}$/,
  emailValidationPattern: /@.*\.(com|co|in|org)$/,
};

export const FieldValidation = {
  firstNameMaxLength: 20,
  lastNameMaxLength: 20,
  emailMaxLength: 50,
  passwordMinLength: 6,
  apartmentNameMaxLength: 20,
  locationMaxLength: 500,
  sizeMinLength: 600,
  sizeMaxLength: 3000,
  roomsMinLength: 1,
  roomsMaxLength: 5,
};

export const NotificationType = {
  success: "success",
  warning: "warning",
  info: "info",
  error: "error",
};
