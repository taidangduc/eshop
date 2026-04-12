import { useState } from "react";
import { getCheckoutOrder, createOrder } from "./api";
import { useQuery, useMutation } from "@tanstack/react-query";
import { useNavigate } from "react-router-dom";
import { PAYMENT_PROVIDERS, SHIPPING_ADDRESS_MODEL } from "./type";

export function useCheckout() {
  const mutation = useUpdateCheckout();

  const navigate = useNavigate();

  const [error, setError] = useState(null);
  const [validated, setValidated] = useState(false);
  const [paymentMethod, setPaymentMethod] = useState(PAYMENT_PROVIDERS[0].id);
  const [paymentProvider, setPaymentProvider] = useState(PAYMENT_PROVIDERS[0]);
  const [shippingAddress, setShippingAddress] = useState(
    SHIPPING_ADDRESS_MODEL,
  );

  const validateException = Object.values(shippingAddress).some(
    (value) => !value || value.trim() === "",
  );

  const changePayment = (methodId) => {
    setPaymentMethod(methodId);
    const provider = PAYMENT_PROVIDERS.find((p) => p.id === methodId);
    setPaymentProvider(provider);
  };

  const redirectWhenSuccess = (res) => {
    if (res?.data?.paymentUrl) {
      window.location.href = res.data.paymentUrl;
      return;
    }
    navigate("/checkout/status?orderNumber= " + res?.data?.orderNumber);
  };

  const showErrorWhenFailure = () => {
    setError("Failed to place order. Please try again.");
  };

  const placeOrder = ({ customerId }) => {
    if (customerId === null) {
      setError("Invalid customer. Please log in again.");
      return;
    }

    if (validateException) {
      setError("Please fill in all required fields.");
      return;
    }

    mutation.mutate(
      {
        customerId: customerId,
        method: paymentProvider.method,
        provider: paymentProvider.provider,
        street: shippingAddress.street,
        city: shippingAddress.city,
        zipCode: shippingAddress.zipCode,
      },
      {
        onSuccess: (res) => {
          redirectWhenSuccess(res);
        },
        onError: () => {
          showErrorWhenFailure();
        },
      },
    );
  };
  return {
    error,
    placeOrder,
    changePayment,
    validated,
    setValidated,
    paymentMethod,
    paymentProvider,
    shippingAddress,
    setShippingAddress,
  };
}

export function useUpdateCheckout() {
  //const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ customerId, method, provider, street, city, zipCode }) =>
      createOrder(customerId, method, provider, street, city, zipCode),
    // onSuccess: () => {
    //   queryClient.invalidateQueries({ queryKey: ["checkout"] });
    // },
    // onError: (error) => {
    //   console.error("Error creating order:", error);
    // },
  });
}

export function useCheckoutStatus(orderNumber) {
  const { data, error, isLoading } = useCheckoutStatusQuery(orderNumber);
  return { data, error, isLoading };
}

export function useCheckoutStatusQuery(orderNumber) {
  return useQuery({
    queryKey: ["checkout_status", orderNumber],
    queryFn: () => getCheckoutOrder(orderNumber),
    enabled: !!orderNumber,
  });
}
