import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { updateBasket, getBasket } from "./api";
import { useMemo, useState } from "react";

export function useBasket() {
  const [confirmedDelete, setConfirmedDelete] = useState(false);
  const [pendingDelete, setPendingDelete] = useState(null);
  const [error, setError] = useState(null);
  const [open, setOpen] = useState(false);

  const query = useBasketQuery();
  const mutation = useUpdateBasket();

  const basket = query.data;
  const totalPrice = useMemo(() => {
    if (!basket) return 0;
    return basket.items.reduce(
      (total, item) => total + item.price * item.quantity,
      0,
    );
  }, [basket]);

  const addToCart = (variantId, quantity) => {
    if (!variantId || quantity <= 0) {
      setError({
        message: "Please select product variation first.",
      });
      return;
    }

    const variant = basket.items.find((item) => item.variantId === variantId);
    const newQuantity = variant ? variant.quantity + quantity : quantity;

    mutation.mutate(
      {
        variantId,
        quantity: newQuantity,
      },
      {
        onError: () => {
          setError({
            id: variantId,
            message: "Failed to add to cart. Please try again.",
          });
        },
        onSuccess: () => {
          setOpen(true);

          setTimeout(() => {
            setOpen(false);
          }, 1000);
        },
      },
    );
  };

  const updateCartItem = (variantId, quantity) => {
    setError(null);
    // if delete variant, show confirmation modal
    if (quantity === 0) {
      const variant = basket.items.find((item) => item.variantId === variantId);
      if (!variant) return;

      setPendingDelete({
        id: variant.variantId,
        title: variant.title,
        name: variant.name,
      });
      setConfirmedDelete(true);
      return;
    }
    mutation.mutate(
      {
        variantId,
        quantity,
      },
      {
        onError: (err) => {
          setError({
            id: variantId,
            message: "Failed to update the basket. Please try again.",
          });
        },
      },
    );
  };

  const confirmDelete = () => {
    if (!pendingDelete) return;
    mutation.mutate(
      {
        variantId: pendingDelete.id,
        quantity: 0,
      },
      {
        onError: (err) => {
          setError({
            id: pendingDelete.id,
            message: "Failed to delete the item. Please try again.",
          });
        },
      },
    );
    setConfirmedDelete(false);
    setPendingDelete(null);
  };

  const cancelDelete = () => {
    setConfirmedDelete(false);
    setPendingDelete(null);
  };

  return {
    basket,
    totalPrice,
    query,
    mutation,
    isFetching: query.isFetching,
    isUpdateing: mutation.isLoading,
    open,
    error,
    confirmedDelete,
    pendingDelete,
    addToCart,
    updateCartItem,
    confirmDelete,
    cancelDelete,
  };
}

export function useBasketQuery() {
  return useQuery({
    queryKey: ["basket"],
    queryFn: () => getBasket().then((res) => res.data),
    initialData: {
      id: "",
      customerId: "",
      items: [],
      createdAt: new Date().toISOString(),
      updatedAt: null,
    },
  });
}

export function useUpdateBasket() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ variantId, quantity }) => updateBasket(variantId, quantity),
    // optimistic update
    onMutate: async (variables) => {
      await queryClient.cancelQueries({ queryKey: ["basket"] });
      const prev = queryClient.getQueryData(["basket"]);

      await queryClient.setQueryData(["basket"], (old) => {
        if (!old) return old;
        return {
          ...old,
          items: old.items
            .map((item) =>
              item.variantId === variables.variantId
                ? { ...item, quantity: variables.quantity }
                : item,
            )
            .filter((item) => item.quantity > 0),
        };
      });
      return { prev };
    },
    // rollback
    onError: (error, variables, context) => {
      if (context?.prev) {
        queryClient.setQueryData(["basket"], context.prev);
      }
    },
    // sync
    onSettled: () => {
      queryClient.invalidateQueries({ queryKey: ["basket"] });
    },
  });
}
