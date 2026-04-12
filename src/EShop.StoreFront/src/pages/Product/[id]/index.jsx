import s from "./index.module.css";
import { useEffect, useRef } from "react";
import { useParams } from "react-router-dom";
import {
  ImagePreview,
  ImageGallery,
  ProductInfo,
  VariantSelector,
  QuantitySelector,
  ProductOverview,
} from "@features/product/components/index";
import clsx from "clsx";
import { Provider } from "../../../features/product/context";
import { useProduct } from "../../../features/product/hook";
import { useBasket } from "../../../features/basket/hook";
import { useCounter } from "../../../hooks/useCounter";

export function ProductDetailPage() {
  const { id } = useParams();
  const hasTrackedRef = useRef(false);

  useEffect(() => {
    if (id && !hasTrackedRef.current) {
      hasTrackedRef.current = true;
    }
  }, [id]);

  const { products, selectedOption, selectOption, variantId, stock, price } = useProduct(id);
  const { error: cart_error, open: cart_modal, addToCart } = useBasket();
  const { count, increase, decrease } = useCounter();

  const hasOneVariant = variantId !== null;
  const limitPhoto = 5;
  
  return (
    <>
      {products && (
        <div className={s["container"]}>
          <div className="flex mt-3 bg-white">
            <Provider images={products?.data?.gallery}>
              {/* Left Section*/}
              <div className={s["detail__section--left"]}>
                <div className="flex flex-col">
                  <ImagePreview />
                  <ImageGallery
                    images={products?.data?.gallery}
                    limit={limitPhoto}
                  />
                </div>
              </div>
              {/* Right Section */}
              <div className={s["detail__section--right"]}>
                <ProductInfo price={price} name={products?.data?.title} />
                <div
                  className={clsx(
                    s["selector__section"],
                    cart_error && s["error"],
                  )}
                >
                  <div className="flex flex-col">
                    <VariantSelector
                      options={products?.data?.options}
                      selectedOption={selectedOption}
                      onChange={selectOption}
                    />
                    <QuantitySelector
                      stock={stock}
                      count={count}
                      onIncrease={increase}
                      onDecrease={decrease}
                      onShow={hasOneVariant}
                    />
                    {cart_error && (
                      <div className={s["error--not-enough-option"]}>
                        {cart_error.message}
                      </div>
                    )}
                  </div>
                </div>
                {/* Action controls */}
                <div className={s["purchase-action__section"]}>
                  <div style={{ paddingLeft: "20px" }}>
                    <div className="flex">
                      {/* Add to cart */}
                      <button
                        onClick={() => addToCart(variantId, count)}
                        className="purchase__button purchase__button-add-to-cart"
                      >
                        <span style={{ marginRight: "5px" }}>
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            width="24"
                            height="24"
                            viewBox="0 0 24 24"
                          >
                            <path
                              fill="currentColor"
                              d="M16 18a2 2 0 0 1 2 2a2 2 0 0 1-2 2a2 2 0 0 1-2-2a2 2 0 0 1 2-2m0 1a1 1 0 0 0-1 1a1 1 0 0 0 1 1a1 1 0 0 0 1-1a1 1 0 0 0-1-1m-9-1a2 2 0 0 1 2 2a2 2 0 0 1-2 2a2 2 0 0 1-2-2a2 2 0 0 1 2-2m0 1a1 1 0 0 0-1 1a1 1 0 0 0 1 1a1 1 0 0 0 1-1a1 1 0 0 0-1-1M18 6H4.27l2.55 6H15c.33 0 .62-.16.8-.4l3-4c.13-.17.2-.38.2-.6a1 1 0 0 0-1-1m-3 7H6.87l-.77 1.56L6 15a1 1 0 0 0 1 1h11v1H7a2 2 0 0 1-2-2a2 2 0 0 1 .25-.97l.72-1.47L2.34 4H1V3h2l.85 2H18a2 2 0 0 1 2 2c0 .5-.17.92-.45 1.26l-2.91 3.89c-.36.51-.96.85-1.64.85"
                            />
                          </svg>
                        </span>
                        <span>Add To Cart</span>
                      </button>
                      {/* Buy now */}
                      {/* <button className="purchase__button purchase__button-buy-now">
                        Buy Now
                      </button> */}
                    </div>
                  </div>
                </div>
              </div>
            </Provider>
          </div>
          <ProductOverview
            category={products?.data?.categoryName}
            description={products?.data?.description}
          />
        </div>
      )}
      {/* Internal Modal */}
      {cart_modal && (
        <div className={s["add-to-cart__modal"]}>
          <div
            className="flex flex-col items-center justify-center "
            style={{ padding: "40px 20px" }}
          >
            <div>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width={60}
                height={60}
                viewBox="0 0 24 24"
              >
                <path
                  fill="#fff"
                  d="m10.6 13.8l-2.15-2.15q-.275-.275-.7-.275t-.7.275t-.275.7t.275.7L9.9 15.9q.3.3.7.3t.7-.3l5.65-5.65q.275-.275.275-.7t-.275-.7t-.7-.275t-.7.275zM12 22q-2.075 0-3.9-.788t-3.175-2.137T2.788 15.9T2 12t.788-3.9t2.137-3.175T8.1 2.788T12 2t3.9.788t3.175 2.137T21.213 8.1T22 12t-.788 3.9t-2.137 3.175t-3.175 2.138T12 22m0-2q3.35 0 5.675-2.325T20 12t-2.325-5.675T12 4T6.325 6.325T4 12t2.325 5.675T12 20m0-8"
                ></path>
              </svg>
            </div>
            <div style={{ marginTop: "10px", color: "white" }}>
              Item has been added to your shopping cart
            </div>
          </div>
        </div>
      )}
    </>
  );
}
