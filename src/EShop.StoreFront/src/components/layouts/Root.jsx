import { Outlet } from "react-router-dom";
import { NavbarLayout } from "./Navbar";
import { HeaderLayout } from "./Header";

export function RootLayout() {
  return (
    <div id="main">
      <div className="flex flex-col">
        <header>
          <NavbarLayout />
          <HeaderLayout />
        </header>
        <main style={{ marginTop: "120px" }}>
          <Outlet />
        </main>
      </div>
    </div>
  );
}
