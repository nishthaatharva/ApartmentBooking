import React from "react";
import { Navigate, Routes, Route } from "react-router-dom";
import LocalStorageService from "../utils/localstorage.service";
import { authPaths } from "../utils/common/route-paths";

const localStorageService = LocalStorageService.getService();
const userInfo = localStorageService.getUser();
const isAdmin = userInfo?.roles === "Administrator";

const AuthGuard: React.FC<React.PropsWithChildren<object>> = ({ children }) => {
  if (!localStorageService.isAuthenticated()) {
    return <Navigate to={"/" + authPaths.signin} />;
  }
  return <>{children}</>;
};

export default AuthGuard;
