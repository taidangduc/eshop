import { apiAuth } from "@/lib/api-auth";

export const getBasket = () => apiAuth.get(`/api/v1/basket`);

export const updateBasket = (variantId, quantity) =>
  apiAuth.post(`/api/v1/basket`, {
    variantId: variantId,
    quantity: quantity,
  });
