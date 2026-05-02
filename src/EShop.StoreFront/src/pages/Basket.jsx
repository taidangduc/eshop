import clsx from "clsx";
import { BasketEmpty, BasketHeader, BasketItem , useBasket} from "@features/basket";
import { NavbarLayout } from "@components/layouts/Navbar";
import { Modal, Table,TableHeader, TableHeaderCell, TableBody, TableRow, TableCell, PageLoading} from "@components/ui";
import { useNavigate } from "react-router-dom";
import { use, useEffect, useState } from "react";
import { formatCurrency } from "@lib/format";

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

  const isFirstLoad = query.isFetching && !query.isFetched;
  const itemInCart = basket?.items?.length;

  const redirectToCheckout = () => navigate("/checkout");

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

  if (isFirstLoad || pageLoading) {
    return <PageLoading />;
  }

  return (
    <>
      <NavbarLayout />
      <BasketHeader />
      {/* content */}
      <div className="w-[1200px] mx-auto py-5">
        {basket && itemInCart === 0 ? (
          <BasketEmpty />
        ) : (
          <div>
            {/* table */}
            <Table>
              <TableHeader>
                <TableRow className="bg-white">
                  <TableHeaderCell className="w-auto text-left">
                    Product
                  </TableHeaderCell>
                  <TableHeaderCell className="w-[15%] text-center"></TableHeaderCell>
                  <TableHeaderCell className="w-[15%] text-center">
                    Unit Price
                  </TableHeaderCell>
                  <TableHeaderCell className="w-[15%] text-center">
                    Quantity
                  </TableHeaderCell>
                  <TableHeaderCell className="w-[15%] text-center">
                    Total Price
                  </TableHeaderCell>
                  <TableHeaderCell className="w-[10%] text-center">
                    Action
                  </TableHeaderCell>
                </TableRow>
                <TableRow>
                  <TableCell colSpan={6}></TableCell>
                </TableRow>
              </TableHeader>
              <TableBody className="border border-gray-300 bg-white">
                <TableRow>
                  <TableCell
                    colSpan={6}
                    className="border border-gray-400 bg-gray-400 font-medium text-white"
                  >
                    Items: {itemInCart}
                  </TableCell>
                </TableRow>

                {basket.items.map((item) => (
                  <BasketItem
                    key={item.variantId}
                    item={item}
                    error={error && error?.id === item.variantId}
                    errorMessage={
                      error?.id === item.variantId ? error.message : ""
                    }
                    isLoading={
                      componentLoading &&
                      mutation.variables?.variantId === item.variantId
                    }
                    onUpdate={(quantity) => {
                      updateCartItem(item.variantId, quantity);
                    }}
                  />
                ))}
              </TableBody>
            </Table>
            {/* toolbar */}
            <div
              className="flex items-center justify-end mt-5 gap-3 bg-white p-5 sticky bottom-0"
              style={{ boxShadow: "0 -4px 8px -4px rgba(0, 0, 0, 0.25)" }}
            >
              <div className="flex gap-3 items-center justify-end font-medium">
                <div className="text-md">
                  Total ({componentLoading ? "0" : itemInCart} item):
                </div>
                <div className="text-2xl">
                  {componentLoading ? "0₫" : formatCurrency(totalPrice)}
                </div>
              </div>
              <button
                className="px-20 py-2 text-white bg-gray-900"
                onClick={redirectToCheckout}
                disabled={itemInCart === 0}
              >
                Checkout
              </button>
            </div>
          </div>
        )}
      </div>
      {/* modal */}
      <Modal open={confirmedDelete} onClose={() => cancelDelete()}>
        <div className="bg-white w-[500px] flex flex-col p-6">
          <h2 className="text-lg font-bold mb-4">
            Do you want to remove this item?
          </h2>
          <div className="mb-10">
            <span>{pendingDelete?.title}</span>
            {pendingDelete?.name && <span>({pendingDelete.name})</span>}
          </div>
          <div className="flex w-full">
            <button
              className="flex-1 border bg-gray-900 text-white py-2 mr-2"
              onClick={() => confirmDelete()}
            >
              Yes
            </button>
            <button
              className="flex-1 border border-gray-900 text-black py-2"
              onClick={() => cancelDelete()}
            >
              No
            </button>
          </div>
        </div>
      </Modal>
    </>
  );
}
