import clsx from "clsx";
import { useEffect, useState } from "react";
import { formatCurrency } from "@lib/format";
import { Modal } from "@components/ui";
import {
  CheckoutHeader,
  ShippingAddress,
  CheckoutSummary,
  PaymentMethod,
  useCheckout,
} from "@features/checkout";
import { NavbarLayout } from "@components/layouts/Navbar";
import { useBasket } from "@features/basket";
import { PAYMENT_PROVIDERS } from "@features/checkout/type";

export function CheckoutPage() {
  const [showCheckoutModal, setShowCheckoutModal] = useState(false);
  const [showModal, setShowModal] = useState(false);

  const { basket, totalPrice } = useBasket();

  const {
    error,
    placeOrder,
    changePayment,
    validated,
    setValidated,
    paymentMethod,
    shippingAddress,
    setShippingAddress,
  } = useCheckout();

  // first load page: open modal if no validated
  useEffect(() => {
    if (!validated) {
      setShowCheckoutModal(true);
    }
  }, []);

  useEffect(() => {
    if (error) {
      setShowModal(true);
    }
  }, [error]);

  const handleChangePayment = (methodId) => {
    changePayment(methodId);
  };

  const handlePlaceOrder = async () => {
    placeOrder({ customerId: basket?.customerId });
  };

  return (
    <>
      <NavbarLayout />
      <CheckoutHeader />
      {/* content */}
      <div className="w-full py-6">
        <div role="main" className="w-[1200px] mx-auto">
          <ShippingAddress
            isShowModal={showCheckoutModal}
            onSetShowModal={setShowCheckoutModal}
            data={shippingAddress}
            status={validated}
            onSetStatus={setValidated}
            onSubmit={setShippingAddress}
          />
          <div className="my-6 bg-white">
            <CheckoutSummary items={basket?.items} />
          </div>
          {/* toolbar */}
          <div className="bg-white">
            <PaymentMethod
              items={PAYMENT_PROVIDERS}
              value={paymentMethod}
              onChange={handleChangePayment}
            />
            <div
              className="flex justify-end items-center gap-5 py-4 px-4"
              style={{ borderTop: "1px dashed rgba(0, 0, 0, 0.09)" }}
            >
              <h3 className="text-md font-medium">Total Payment:</h3>
              <div className="text-3xl">{formatCurrency(totalPrice)}</div>
            </div>
            <div
              className="flex justify-end items-center gap-5 py-4 px-4"
              style={{ borderTop: "1px dashed rgba(0, 0, 0, 0.09)" }}
            >
              <button
                onClick={() => {
                  handlePlaceOrder();
                }}
                className="px-20 py-2 text-white bg-gray-900"
              >
                Place Order
              </button>
            </div>
          </div>
        </div>
      </div>
      {/* modal */}
      {error && (
        <Modal open={showModal} onClose={() => setShowModal(false)}>
          <div className="bg-white w-[500px] flex flex-col p-6">
            <h2 className="text-lg font-bold mb-4">Error !!!</h2>
            <div className="mb-10">
              <span>
                We couldn't process your order. Please try again later.
                {error && <div className="mt-2">{error}</div>}
              </span>
            </div>
            <div className="flex w-full">
              <button
                className="flex-1 border bg-gray-900 text-white py-2 mr-2"
                onClick={() => setShowModal(false)}
              >
                OK, got it
              </button>
            </div>
          </div>
        </Modal>
      )}
    </>
  );
}
