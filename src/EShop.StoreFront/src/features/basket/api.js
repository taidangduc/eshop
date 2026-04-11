import { authClient } from "../../lib/auth-client";

export const getBasket = () => authClient.get(`/api/v1/basket`);

export const updateBasket = (variantId, quantity) =>
  authClient.post(`/api/v1/basket`, {
    variantId: variantId,
    quantity: quantity,
  });
