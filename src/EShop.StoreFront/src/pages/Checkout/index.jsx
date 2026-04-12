import s from "./index.module.css";
import clsx from "clsx";
import { useEffect, useState } from "react";
import { formatCurrency } from "@/lib/format";
import { Modal } from "@/components/ui";
import {
  CheckoutHeader,
  ShippingAddress,
  CheckoutSummary,
  PaymentMethod,
} from "@features/checkout/components";
import { NavbarLayout } from "../../components/layouts/Navbar";
import { useBasket } from "../../features/basket/hook";
import { PAYMENT_PROVIDERS } from "../../features/checkout/type";
import { useCheckout } from "../../features/checkout/hook";

export function CheckoutPage() {
  const [showModal, setShowModal] = useState(false);
  const [showGlobalModal, setShowGlobalModal] = useState(false);

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
      setShowModal(true);
    }
  }, []);

  useEffect(() => {
    if (error) {
      setShowGlobalModal(true);
    }
  }, [error]);

  const handleChangePayment = (methodId) => {
    changePayment(methodId);
  };

  const handlePlaceOrder = async () => {
    placeOrder({ customerId: basket?.customerId });
  };

  return (
    <div>
      {/* HEADER */}
      <div className="bg-white" style={{ marginBottom: "12px" }}>
        <NavbarLayout />
        <CheckoutHeader />
      </div>
      {/* CONTENT */}
      <div>
        <div
          role="main"
          className="container-wrapper mx-auto"
          style={{ fontSize: "14px", lineHeight: "16.8px" }}
        >
          <ShippingAddress
            isShowModal={showModal}
            onSetShowModal={setShowModal}
            data={shippingAddress}
            status={validated}
            onSetStatus={setValidated}
            onSubmit={setShippingAddress}
          />
          <div style={{ marginTop: "12px", backgroundColor: "white" }}>
            <CheckoutSummary items={basket?.items} />
          </div>
          <div className={s["checkout-section__footer"]}>
            <div className={s["checkout-footer-with-payment-section"]}>
              <PaymentMethod
                items={PAYMENT_PROVIDERS}
                value={paymentMethod}
                onChange={handleChangePayment}
              />
            </div>
            <div className={s["checkout-footer"]}>
              <h3
                className={clsx(
                  s["checkout-footer-grid-per-row"],
                  s["checkout-footer__title"],
                )}
              >
                Total Payment
              </h3>
              <div
                className={clsx(
                  s["checkout-footer-grid-per-row"],
                  s["checkout-footer-total-price"],
                )}
              >
                {formatCurrency(totalPrice)}
              </div>
              <div
                className={clsx(
                  s["checkout-footer-grid-per-row"],
                  s["checkout-footer-with-button-order"],
                )}
              >
                <div></div>
                <button
                  onClick={() => {
                    handlePlaceOrder();
                  }}
                  className={s["checkout-footer-button-order"]}
                >
                  Place Order
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
      {/* GLOBAL MODAL */}
      {error && (
        <Modal open={showGlobalModal} onClose={() => setShowGlobalModal(false)}>
          <div className={s["w4p-container"]}>
            <div className={s["w4p-wrapper"]}>
              <div className={s["w4p-box"]}>
                <div className={s["w4p-box__subtitle"]}>
                  <p>
                    Oops! We couldn’t process your order. Please check the
                    following:
                    <br />
                    1. All items in your order must use the same payment method.
                    <br />
                    2. Please review your delivery address.
                    <br />
                    3. Please review your payment details or choose a different
                    payment option.
                  </p>
                </div>
                <div className="flex w-100">
                  <button
                    onClick={() => setShowGlobalModal(false)}
                    className={s["w4p__button"]}
                  >
                    OK, got it
                  </button>
                </div>
              </div>
            </div>
          </div>
        </Modal>
      )}
    </div>
  );
}
