export class APIs {
  //Auth
  static signinApi = "/auth/signin";
  static refreshTokenApi = "/auth/refreshtoken";

  //User
  static getUserApi = "/users/";
  static updateUserApi = "/users/";
  static inviteUserApi = "/auth/register";
  static searchUserApi = "/users/search";
  static deleteUserApi = "/users/";

  //Apartment
  static searchApartmentApi = "/apartment/search";
  static deleteApartmentApi = "/apartment/";
  static getApartmentApi = "/apartment/";
  static addApartment = "/apartment/create";
  static updateApartment = "/apartment/";

  //booking
  static bookApartmentApi = "/booking/book-apartment";
}
