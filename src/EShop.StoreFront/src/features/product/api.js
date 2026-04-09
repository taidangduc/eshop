import { apiClient } from "@/lib/api-client";

export const getProducts = (pageIndex, pageSize) =>
  apiClient.get(`/api/v1/products`);

export const getProduct = (id) => apiClient.get(`/api/v1/products/${id}`);

export const getVariantByOptions = (productId, optionValueIds) =>
  apiClient.post(`/api/v1/variants/by-options`, { productId, optionValueIds });
