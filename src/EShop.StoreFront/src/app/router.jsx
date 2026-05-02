import { BasketPage } from "@pages/Basket";
import { ProductDetailPage } from "@pages/ProductDetail";
import { CheckoutPage } from "@pages/Checkout";
import { CheckoutStatusPage } from "@pages/CheckoutStatus";
import { NotFoundPage } from "@pages/NotFound";
import { HomePage } from "@pages/Home";
import { RootLayout } from "@components/layouts/Root";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { AuthGuard } from "@features/auth/AuthGuard";

const routes = [
  {
    element: <RootLayout />,
    children: [
      { path: "/", element: <HomePage /> },
      { path: "/product/:id", element: <AuthGuard><ProductDetailPage /></AuthGuard> },
    ],
    errorElement: <NotFoundPage />,
  },
  { path: "/cart", element: <AuthGuard><BasketPage /></AuthGuard> },
  { path: "/checkout", element: <AuthGuard><CheckoutPage /></AuthGuard> },
  { path: "/checkout/:status", element: <AuthGuard><CheckoutStatusPage /></AuthGuard> },
  { path: "*", element: <NotFoundPage /> },
];

const router = createBrowserRouter(routes);

export function AppRouter() {
  return <RouterProvider router={router} />;
}
