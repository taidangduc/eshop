import { Outlet } from "react-router-dom";
import { SideBar } from "./components/SideBar";

export default function AdminLayout() {
  return (
    <div id="main">
      <div>
        <SideBar />
        <main className="flex-1 ml-[300px] bg-gray-100">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
