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
import { useEffect, useState } from "react";
import { formatCurrency } from "@/lib/format";
import {
  BasketItem,
  BasketEmpty,
  BasketHeader,
} from "@features/basket/components/index";
import { NavbarLayout } from "../../components/layouts/Navbar";
import { useBasket } from "../../features/basket/hook";

export function BasketPage() {
  const navigate = useNavigate();

  const [componentLoading, setComponentLoading] = useState(false);
  const [pageLoading, setPageLoading] = useState(false);

  const {
    basket,
    totalPrice,
    query,
    mutation,
    error,
    confirmedDelete,
    pendingDelete,
    updateCartItem,
    confirmDelete,
    cancelDelete,
  } = useBasket();

  const is_first_load = query.isFetching && !query.isFetched;
  const item_in_cart = basket?.items?.length;

  const redirectToCheckout = () => {
    navigate("/checkout");
  };

  // authentication
  // useEffect(() => {
  //   if (profileStorage.get() === null || undefined) {
  //     navigate("/login");
  //   }
  // }, [basket]);

  // wait component loading when update basket item
  useEffect(() => {
    setComponentLoading(true);
    const timeout = setTimeout(() => {
      setComponentLoading(false);
    }, 350);

    return () => clearTimeout(timeout);
  }, [query.isFetching]);

  // wait page loading when first load or refresh page
  useEffect(() => {
    setPageLoading(true);
    const timeout = setTimeout(() => {
      setPageLoading(false);
    }, 1000);
    return () => clearTimeout(timeout);
  }, [query.isFetched]);

  if (is_first_load || pageLoading) {
    return <PageLoading />;
  }

  return (
    <div>
      <NavbarLayout />
      <BasketHeader />
      <div>
        <div className="mx-auto container-wrapper">
          {basket && item_in_cart === 0 ? (
            <>
              <BasketEmpty />
            </>
          ) : (
            <>
              <div className="flex flex-col pt-[20px]">
                <Table>
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
                  <TableBody className={s["basket__table-content"]}>
                    <section className={s["table-content__section"]}>
                      <div className={s["table-content__title"]}>
                        <span>
                          Items: {componentLoading ? "0" : item_in_cart}
                        </span>
                      </div>
                      <div>
                        {basket.items.map((item, index) => (
                          <div key={item.variantId}>
                            <BasketItem
                              item={item}
                              error={error && error?.id === item.variantId}
                              errorMessage={
                                error?.id === item.variantId
                                  ? error.message
                                  : ""
                              }
                              isLoading={
                                componentLoading &&
                                mutation.variables?.variantId === item.variantId
                              }
                              onUpdate={(quantity) => {
                                updateCartItem(item.variantId, quantity);
                              }}
                            />
                            {index < item_in_cart - 1 && (
                              <div className={s["basket__item-divider"]} />
                            )}
                          </div>
                        ))}
                      </div>
                    </section>
                  </TableBody>
                </Table>
              </div>
              {/* Footer */}
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
                  <div></div>
                  <div className="flex-1"></div>
                  <div className="flex flex-col">
                    <div className="flex items-center flex-end">
                      <div
                        className={clsx(
                          "flex items-center",
                          s["basket__footer-total-title"],
                        )}
                      >
                        Total ({componentLoading ? "0" : basket.items.length}{" "}
                        item):
                      </div>
                      <div className={clsx(s["basket__footer-total-subtitle"])}>
                        {componentLoading ? "0₫" : formatCurrency(totalPrice)}
                      </div>
                    </div>
                  </div>
                  {/* button */}
                  <div>
                    <button
                      onClick={() => redirectToCheckout()}
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
      {/* Global modal when remove basket item */}
      <Modal
        open={confirmedDelete}
        onClose={() => {
          cancelDelete();
        }}
      >
        <div className="bg-white p-6 rounded-[2px] flex flex-col w-[500px]">
          <h2 className="text-[20px] pb-[40px]">
            Do you want to remove this item?
          </h2>
          <div className="text-[16px]">
            <span>{pendingDelete?.title} </span>
            {pendingDelete?.name && <span>({pendingDelete?.name})</span>}
          </div>
          <div className="flex w-full pt-[60px]">
            <button
              style={{
                backgroundColor: "black",
                color: "white",
                borderRadius: "2px",
              }}
              className="flex-1 m-1 h-[40px]"
              onClick={() => {
                confirmDelete();
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
                cancelDelete();
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
