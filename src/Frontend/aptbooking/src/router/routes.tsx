import { ReactElement, lazy } from "react";
import { authPaths, appPaths } from "../utils/common/route-paths";
import LocalStorageService from "../utils/localstorage.service";
const Users = lazy(() => import("../pages/user/pages/list-user"));
const SignIn = lazy(() => import("../pages/auth/pages/sign-in"));
const Apartments = lazy(
  () => import("../pages/apartment/pages/list-apartments")
);

interface Route {
  path: string;
  element: ReactElement;
  layout: string;
}

const localStorageService = LocalStorageService.getService();
const userInfo = localStorageService.getUser();
const isAdmin = userInfo?.roles === "Administrator";
const authRoutes: Route[] = [
  {
    path: "/" + authPaths.signin,
    element: <SignIn />,
    layout: "blank",
  },
];

let appRoutes: Route[] = [];
if (userInfo !== null) {
  if (isAdmin) {
    appRoutes = [
      {
        path: "/",
        element: <Users />,
        layout: "",
      },
      {
        path: "/" + appPaths.users,
        element: <Users />,
        layout: "",
      },
      {
        path: "/" + appPaths.apartments,
        element: <Apartments />,
        layout: "",
      },
    ];
  } else {
    appRoutes = [
      {
        path: "/",
        element: <Apartments />,
        layout: "",
      },
      {
        path: "/" + appPaths.apartments,
        element: <Apartments />,
        layout: "",
      },
    ];
  }
}

//Always include apartment route regardless of roles
// appRoutes.push({
//   path: "/" + appPaths.apartments,
//   element: <Apartments />,
//   layout: "",
// });

const routes: Route[] = [...authRoutes, ...appRoutes];

export { routes };