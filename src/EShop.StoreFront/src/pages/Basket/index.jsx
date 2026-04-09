import NavBar from "@/components/layouts/storefront/components/Navbar/Navbar";
import s from "./index.module.css";
import clsx from "clsx";
import {
  Modal,
  Table,
  TableHeader,
  TableHeaderCell,
  TableBody,
  PageLoading,
} from "@/components/ui";
import { useNavigate } from "react-router-dom";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { getBasket, updateBasket } from "@features/basket/api";
import { useEffect, useState } from "react";
import { formatCurrency } from "@/lib/format";

import {
  BasketItem,
  BasketEmpty,
  BasketHeader,
} from "@features/basket/components/index";

export function BasketPage() {
  const navigate = useNavigate();
  const queryClient = useQueryClient();

  //re-load page after isFetching
  const [showLoading, setShowLoading] = useState(false);
  const [showItemLoading, setShowItemLoading] = useState(false);
  const [hasError, setHasError] = useState({
    isError: false,
    id: null,
    message: "",
  });

  const [confirmDelete, setConfirmDelete] = useState(false);
  const [pendingDelete, setPendingDelete] = useState({
    variantId: null,
    title: "",
    name: "",
  });

  const {
    data: basket,
    isFetching,
    isFetched,
  } = useQuery({
    queryKey: ["basket"],
    queryFn: () => getBasket().then((res) => res.data),
    retry: false,
    refetchOnWindowFocus: false,
    initialData: {
      id: "",
      customerId: "",
      items: [],
      createdAt: new Date().toDateString(),
      lastModified: null,
    },
  });

  // mutation basket state
  const updateMutation = useMutation({
    mutationFn: ({ variantId, quantity }) => updateBasket(variantId, quantity),

    onMutate: async ({ variantId, quantity }) => {
      await queryClient.cancelQueries({ queryKey: ["basket"] });

      const previousBasket = queryClient.getQueryData(["basket"]);

      // optimistic update
      queryClient.setQueryData(["basket"], (old) => {
        if (!old) return old;

        return {
          ...old,
          items: old.items
            .map((item) =>
              item.variantId === variantId ? { ...item, quantity } : item,
            )
            .filter((item) => item.quantity > 0),
        };
      });

      return { previousBasket, variantId };
    },
    onSuccess: () => {
      setHasError({
        isError: false,
        id: null,
        message: "",
      });
    },
    onError: (err, variables, context) => {
      if (context?.previousBasket) {
        queryClient.setQueryData(["basket"], context.previousBasket);
      }
      setHasError({
        isError: true,
        id: variables.variantId,
        message: "Invalid input",
      });
    },

    onSettled: () => {
      queryClient.invalidateQueries({ queryKey: ["basket"] });
    },
  });

  // mutation price: reduce(func, initValue)
  const totalPrice = basket.items?.reduce(
    (sum, item) => (sum = sum + item.price * item.quantity),
    0,
  );

  const handleUpdateBasket = (variantId, quantity) => {
    if (quantity === 0) {
      setConfirmDelete(true);

      const variant = basket.items.find((item) => item.variantId === variantId);
      if (!variant) return;

      setPendingDelete({
        variantId: variant.variantId,
        title: variant.title,
        name: variant.name,
      });

      return;
    }

    updateMutation.mutate({ variantId, quantity });
  };

  const handleConfirmDelete = () => {
    if (!pendingDelete) return;

    updateMutation.mutate({ variantId: pendingDelete.variantId, quantity: 0 });

    setConfirmDelete(false);
    setPendingDelete({
      variantId: null,
      title: "",
      name: "",
    });
  };

  // authentication
  // useEffect(() => {
  //   if (profileStorage.get() === null || undefined) {
  //     navigate("/login");
  //   }
  // }, [basket]);

  // checkout func
  const handleCheckout = () => {
    navigate("/checkout");
  };

  const isFirstLoad = isFetching && !isFetched;

  // item load
  useEffect(() => {
    setShowItemLoading(true);
    const timer = setTimeout(() => {
      setShowItemLoading(false);
    }, 350);

    return () => clearTimeout(timer);
  }, [isFetching]);

  // page load
  useEffect(() => {
    setShowLoading(true);
    const timer = setTimeout(() => {
      setShowLoading(false);
    }, 1000);

    return () => clearTimeout(timer);
  }, [isFetched]);

  if (isFirstLoad || showLoading) {
    return <PageLoading />;
  }

  return (
    <div>
      <NavBar />
      <BasketHeader />
      <div>
        <div className="mx-auto container-wrapper">
          {basket && basket.items.length === 0 ? (
            <>
              <BasketEmpty />
            </>
          ) : (
            <>
              <div className="flex flex-col pt-[20px]">
                {/* Table */}
                <Table>
                  {/* Table Header */}
                  <TableHeader className={s["basket__table-header"]}>
                    <TableHeaderCell
                      className={s["div-checkbox"]}
                      flex="0 0 58px"
                    >
                      <label htmlFor="">
                        <input type="text" hidden />
                        <div className={s["div-checkbox-wrap-input"]}></div>
                      </label>
                    </TableHeaderCell>
                    <TableHeaderCell flex="3.5">Product</TableHeaderCell>
                    <TableHeaderCell flex="1.75"></TableHeaderCell>
                    <TableHeaderCell flex="2" align="center">
                      Unit Price
                    </TableHeaderCell>
                    <TableHeaderCell flex="2" align="center">
                      Quantity
                    </TableHeaderCell>
                    <TableHeaderCell flex="1.5" align="center">
                      Total Price
                    </TableHeaderCell>
                    <TableHeaderCell flex="1.75" align="center">
                      Actions
                    </TableHeaderCell>
                  </TableHeader>

                  {/* Table Body */}
                  <TableBody className={s["basket__table-content"]}>
                    <section className={s["table-content__section"]}>
                      {/* Title */}
                      <div className={s["table-content__title"]}>
                        <span>
                          Items: {showItemLoading ? "0" : basket.items.length}
                        </span>
                      </div>
                      {/* CartItem */}
                      <div>
                        {basket && basket.items.length === 0 ? (
                          <>
                            <span></span>
                          </>
                        ) : (
                          <>
                            {basket.items.map((item, index) => (
                              <div key={item.variantId}>
                                <BasketItem
                                  item={item}
                                  error={
                                    hasError.isError &&
                                    hasError.id === item.variantId
                                  }
                                  errorMessage={hasError.message}
                                  isUpdating={
                                    showItemLoading &&
                                    updateMutation.variables?.variantId ===
                                      item.variantId
                                  }
                                  onUpdate={(quantity) => {
                                    handleUpdateBasket(
                                      item.variantId,
                                      quantity,
                                    );
                                  }}
                                />
                                {index < basket.items.length - 1 && (
                                  <div className={s["basket__item-divider"]} />
                                )}
                              </div>
                            ))}
                          </>
                        )}
                      </div>
                    </section>
                  </TableBody>
                </Table>
              </div>
              {/* basket footer */}
              <section className={s["basket__footer"]}>
                {/* promotion */}
                <div className={s["basket__footer-promotion"]}>
                  <img
                    style={{ marginRight: "8px" }}
                    src="src/public/voucher_icon.svg"
                  />
                  <div>Platform voucher</div>
                  <div className="flex-1"></div>
                  <button className={s["basket__footer-promotion-button"]}>
                    Select or enter code
                  </button>
                </div>
                {/*  */}
                <div className={s["basket__footer-divider"]}></div>
                {/* total */}
                <div className={s["basket__footer-total"]}>
                  {/* selection */}
                  <div className={clsx(s["div-checkbox"])}>
                    <label htmlFor="">
                      <input type="text" hidden />
                      <div className={s["div-checkbox-wrap-input"]}></div>
                    </label>
                  </div>
                  <button className={s["basket__footer--selected"]}>
                    Select All (0)
                  </button>
                  <button className={s["basket__footer--unselected"]}>
                    Delete
                  </button>
                  {/*  */}
                  <div></div>
                  {/* text */}
                  <div className="flex-1"></div>
                  <div className="flex flex-col">
                    <div className="flex items-center flex-end">
                      <div
                        className={clsx(
                          "flex items-center",
                          s["basket__footer-total-title"],
                        )}
                      >
                        Total ({showItemLoading ? "0" : basket.items.length}{" "}
                        item):
                      </div>
                      <div className={clsx(s["basket__footer-total-subtitle"])}>
                        {showItemLoading ? "0₫" : formatCurrency(totalPrice)}
                      </div>
                    </div>
                  </div>
                  {/* button */}
                  <div>
                    <button
                      onClick={() => handleCheckout()}
                      className={s["basket__footer-button"]}
                    >
                      <span className={s["basket__footer-button-title"]}>
                        Checkout
                      </span>
                    </button>
                  </div>
                </div>
              </section>
            </>
          )}
        </div>
      </div>
      {/* Modal when remove basket item */}
      <Modal
        open={confirmDelete}
        onClose={() => {
          setConfirmDelete(false);
        }}
      >
        <div className="bg-white p-6 rounded-[2px] flex flex-col w-[500px]">
          <h2 className="text-[20px] pb-[40px]">
            Do you want to remove this item?
          </h2>
          <div className="text-[16px]">
            <span>{pendingDelete.title} </span>
            {pendingDelete.name && <span>({pendingDelete.name})</span>}
          </div>
          <div className="flex w-full pt-[60px]">
            <button
              style={{
                backgroundColor: "black",
                color: "white",
                borderRadius: "2px",
              }}
              className=" flex-1 m-1 h-[40px]"
              onClick={() => {
                handleConfirmDelete();
              }}
            >
              Yes
            </button>
            <button
              style={{
                border: "1px solid rgb(204, 204, 204)",
                color: "black",
                borderRadius: "2px",
              }}
              className="bg-blue-500 flex-1 m-1 "
              onClick={() => {
                setConfirmDelete(false);
              }}
            >
              No
            </button>
          </div>
        </div>
      </Modal>
    </div>
  );
}
