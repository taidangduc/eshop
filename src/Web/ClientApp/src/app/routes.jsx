import HomePage from "@/features/home/HomePage";
import { BasketPage } from "../features/basket/pages/Basket/BasketPage";
import { ProductDetailPage } from "../features/catalog/product/pages/ProductDetail/ProductDetailPage";

import NotFound from "./not-found";

import { CheckoutPage } from "../features/ordering/checkout/pages/Checkout/CheckoutPage";
import { CheckoutResultPage } from "../features/ordering/checkout/pages/CheckoutResult/CheckoutResultPage";
import RootLayout from "../layouts/storefront/layout";
import { ProductEditorPage } from "../features/catalog/product/pages/ProductEditor/ProductEditorPage";
import AdminLayout from "../layouts/backoffice/layout";

export const routes = [
  // root routes
  {
    element: <RootLayout />,
    children: [
      { path: "/", element: <HomePage /> },
      { path: "/product/:id", element: <ProductDetailPage /> },
    ],
    errorElement: <NotFound />,
  },
  { path: "/cart", element: <BasketPage /> },
  { path: "/checkout", element: <CheckoutPage /> },
  { path: "/checkout/result", element: <CheckoutResultPage /> },

  // admin routes
  {
    element: <AdminLayout />,
    children: [
      { path: "/portal/product/new", element: <ProductEditorPage /> },
      { path: "/portal/product/list", element: <ProductEditorPage /> },
    ],
    errorElement: <NotFound />,
  },
];
