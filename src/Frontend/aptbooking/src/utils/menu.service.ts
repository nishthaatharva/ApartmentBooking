import { appPaths } from "./common/route-paths";
import LocalStorageService from "./localstorage.service";

class MenuService {
  getMenus(): Array<{ label: string; link: string }> {
    const localStorageService = LocalStorageService.getService();
    const userInfo = localStorageService.getUser();
    const isAdmin = userInfo?.roles === "Administrator";
    const menuItems: Array<{ label: string; link: string }> = [];

    if (isAdmin) {
      menuItems.push({
        label: "Apartments",
        link: "/" + appPaths.apartments,
      });
      menuItems.push({
        label: "Users",
        link: "/" + appPaths.users,
      });
    } else {
      menuItems.push({
        label: "Apartments",
        link: "/" + appPaths.apartments,
      });
    }

    return menuItems;
  }
}
export default new MenuService();
