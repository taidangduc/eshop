import { Outlet } from "react-router-dom";
import { NavbarLayout } from "./Navbar";
import { HeaderLayout } from "./Header";

export function RootLayout() {
  return (
    <div id="main">
      <div>
        <NavbarLayout />
        <HeaderLayout />
        <main>
          <Outlet />
        </main>
      </div>
    </div>
  );
}
