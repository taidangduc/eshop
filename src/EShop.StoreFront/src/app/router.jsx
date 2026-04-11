import { BasketPage } from "@pages/basket/index";
import { ProductDetailPage } from "@pages/product/[id]/index";
import { CheckoutPage } from "@pages/checkout/index";
import { CheckoutStatusPage } from "@pages/checkout/[status]/index";
import { NotFoundPage } from "@pages/NotFound";
import { HomePage } from "@pages/home/index";
import { RootLayout } from "@components/layouts/Root";
import { createBrowserRouter, RouterProvider } from "react-router-dom";

const routes = [
  {
    element: <RootLayout />,
    children: [
      { path: "/", element: <HomePage /> },
      { path: "/product/:id", element: <ProductDetailPage /> },
    ],
    errorElement: <NotFoundPage />,
  },
  { path: "/cart", element: <BasketPage /> },
  { path: "/checkout", element: <CheckoutPage /> },
  { path: "/checkout/:status", element: <CheckoutStatusPage /> },
  { path: "*", element: <NotFoundPage /> },
];

const router = createBrowserRouter(routes);

export function AppRouter() {
  return <RouterProvider router={router} />;
}
