import { authClient } from "@/lib/auth-client";
import { apiClient } from "@/lib/api-client";

export const getOrder = () => authClient.get(`/api/v1/orders`);

export const getCheckoutOrder = (orderNumber) =>
  authClient.get(`/api/v1/orders/checkout/${orderNumber}`);

export const createOrder = (
  _customerId,
  _method,
  _provider,
  _street,
  _city,
  _zipCode,
) =>
  authClient.post(`/api/v1/orders`, {
    customerId: _customerId,
    method: _method,
    provider: _provider,
    street: _street,
    city: _city,
    zipCode: _zipCode,
  });

export const createPaymentUrl = (_orderNumber, _amount, _provider) =>
  apiClient.post(`/api/v1/payment/create`, {
    orderNumber: _orderNumber,
    amount: _amount,
    provider: _provider,
  });
