import { ProductText } from "./ProductText";
import { ProductPrice } from "./ProductPrice";

export function ProductInfo({ price, name, isPriceLoading }) {
  return (
    <>
      <ProductText name={name} />
      <ProductPrice price={price} isPriceLoading={isPriceLoading} />
    </>
  );
}
