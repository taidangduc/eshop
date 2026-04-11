import { useState } from "react";
import { getCheckoutOrder, createOrder, createPaymentUrl } from "./api";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useNavigate } from "react-router-dom";
import { PAYMENT_PROVIDERS, CHECKOUT_MODEL } from "./type";

export function useCheckout() {
  const mutation = useUpdateCheckout();

  const navigate = useNavigate();

  const [error, setError] = useState(null);
  const [checkoutData, setCheckoutData] = useState(CHECKOUT_MODEL);

  const changePaymentMethod = ({ methodId, provider }) => {
    const isProvider = PAYMENT_PROVIDERS.find((p) => p.id === provider);
    if (!isProvider) {
      console.error("Invalid payment provider selected");
      return;
    }

    setCheckoutData((prev) => ({
      ...prev,
      method: methodId,
      provider,
    }));
  };

  const redirectWhenSuccess = (res) => {
    if (
      res?.paymentProvider?.method === 2 &&
      res?.paymentProvider?.provider !== 0
    ) {
      try {
        const orderNumber = res?.orderNumber;
        const amount = res?.amount || 0;
        const response = createPaymentUrl(
          orderNumber,
          amount,
          res?.paymentProvider,
        );
        var payment = response?.data;

        if (!payment.status) {
          setError(payment.error);
        }
        const paymentUrl = payment?.data || "";
        if (paymentUrl) {
          window.location.href = paymentUrl;
        }
      } catch {
        setError("Failed to create payment URL. Please try again.");
      }
    } else {
      navigate("/checkout/status?orderNumber=" + res?.orderNumber);
    }
  };

  const showErrorWhenFailure = () => {
    setError("Failed to place order. Please try again.");
  };

  const placeOrder = ({
    customerId,
    method,
    provider,
    street,
    city,
    zipCode,
  }) => {
    if (!customerId || !method || !provider || !street || !city || !zipCode) {
      console.error("Missing required fields for placing order");
      return;
    }

    mutation.mutate(
      { customerId, method, provider, street, city, zipCode },
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
    checkoutData,
    error,
    changePaymentMethod,
    placeOrder,
  };
}

export function useUpdateCheckout() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ customerId, method, provider, street, city, zipCode }) =>
      createOrder(customerId, method, provider, street, city, zipCode),
    onSuccess: () => {
      // Invalidate and refetch
      queryClient.invalidateQueries({ queryKey: ["checkout"] });
    },
    onError: (error) => {
      console.error("Error creating order:", error);
    },
  });
}

export function useCheckoutStatus(orderNumber) {
  const { data, error, isLoading } = useCheckoutStatusQuery(orderNumber);
  return { data, error, isLoading };
}

export function useCheckoutStatusQuery(orderNumber) {
  return useQuery({
    queryKey: ["checkout", orderNumber],
    queryFn: () => getCheckoutOrder(orderNumber),
    enabled: !!orderNumber,
  });
}
