import React, { Suspense } from "react";
import ReactDOM from "react-dom/client";

// Tailwind css
import "./tailwind.css";

// Router
import { RouterProvider } from "react-router-dom";
import router from "./router/index";

// Redux
import { Provider } from "react-redux";
import store from "./store/index";

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <Suspense>
      <Provider store={store}>
        {/* <GlobalContextProvider> */}
        <RouterProvider router={router} />
        {/* </GlobalContextProvider> */}
      </Provider>
    </Suspense>
  </React.StrictMode>
);
