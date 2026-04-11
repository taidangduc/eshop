import React, { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./styles/global.css";
import "./styles/index.css";
import { Provider } from "./app/provider";
import { AppRouter } from "./app/router";

createRoot(document.getElementById("main")).render(
  <React.StrictMode>
    <Provider>
      <AppRouter />
    </Provider>
  </React.StrictMode>
);
