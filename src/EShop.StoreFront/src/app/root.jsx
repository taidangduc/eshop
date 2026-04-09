import { routes } from "./router";
import { RouterProvider, createBrowserRouter } from "react-router-dom";

const router = createBrowserRouter(routes);

function Root(){
    return <RouterProvider router={router} />
}
export default Root;    