import s from "./BasketItem.module.css";
import clsx from "clsx";
import { formatCurrency } from "@/lib/format";
import {
  TableRow,
  TableCell,
  CardSkeleton,
  TextSkeleton,
} from "@/components/ui";
import { Image } from "@/components/ui";
import { Link } from "react-router-dom";

export function BasketItem({ item, onUpdate, isLoading, error, errorMessage }) {
  const totalPrice = (price, quantity) => price * quantity;

  if (isLoading) {
    return (
      <div className={s["basket-item"]} role="listitem">
        <TableRow>
          <TableCell className={s["div-checkbox"]} flex="0 0 58px">
            <label htmlFor="">
              <input type="text" hidden />
              <div className={s["div-checkbox-wrap-input"]}></div>
            </label>
          </TableCell>
          <TableCell flex="3.5">
            <CardSkeleton className={s["product__image"]} />
            <div className={s["product__info"]}>
              <TextSkeleton />
            </div>
          </TableCell>
          <TableCell
            className={"flex-col items-start"}
            flex="1.75"
            align="center"
          >
            <TextSkeleton className="card-body" />
            <TextSkeleton className="card-body mt-[5px]" />
          </TableCell>
          <TableCell flex="2" align="center">
            <TextSkeleton className="card-body" />
          </TableCell>
          <TableCell flex="2" align="center">
            <CardSkeleton className="card-input" />
          </TableCell>
          <TableCell flex="1.5" align="center">
            <TextSkeleton className="card-body" />
          </TableCell>
          <TableCell flex="1.75" align="center">
            <CardSkeleton className="card-body card-button" />
          </TableCell>
        </TableRow>
      </div>
    );
  }

  return (
    <div className={s["basket-item"]} role="listitem">
      <TableRow>
        {/* Checkbox */}
        <TableCell className={s["div-checkbox"]} flex="0 0 58px">
          <label htmlFor="">
            <input type="text" hidden />
            <div className={s["div-checkbox-wrap-input"]}></div>
          </label>
        </TableCell>
        {/* Product */}
        <TableCell flex="3.5">
          <Link>
            <Image
              src={item.imageUrl}
              className={s["product__image"]}
              alt={item.title}
            />
          </Link>
          <div className={s["product__info"]}>
            <Link className={clsx("line-clamp-2", s["product__title"])}>
              {item.title}
            </Link>
          </div>
        </TableCell>
        {/* Variant */}
        <TableCell flex="1.75" align="center">
          {item.name && (
            <button type="button" className={s["product-variant-selection"]}>
              <div>Variants:</div>
              <div style={{ marginTop: "5px" }}>{item.name}</div>
            </button>
          )}
        </TableCell>
        {/* Price */}
        <TableCell flex="2" align="center">
          {formatCurrency(item.price)}
        </TableCell>
        {/* Quantity */}
        <TableCell className={clsx("relative")} flex="2" align="center">
          <div className={s["quantity-selector__button-wrapper"]}>
            <button
              type="button"
              aria-label="Decrease"
              onClick={() => onUpdate(item.quantity - 1)}
              className={s["quantity-selector__button"]}
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="24"
                height="24"
                viewBox="0 0 24 24"
              >
                <path
                  fill="currentColor"
                  d="M18 12.998H6a1 1 0 0 1 0-2h12a1 1 0 0 1 0 2"
                />
              </svg>
            </button>
            <input
              aria-label="search-input"
              type="text"
              value={item.quantity}
              readOnly
              className={s["quantity-selector__input"]}
            />
            <button
              type="button"
              aria-label="Increase"
              onClick={() => onUpdate(item.quantity + 1)}
              className={s["quantity-selector__button"]}
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="24"
                height="24"
                viewBox="0 0 24 24"
              >
                <path
                  fill="currentColor"
                  d="M19 12.998h-6v6h-2v-6H5v-2h6v-6h2v6h6z"
                />
              </svg>
            </button>
          </div>
          {/* Error message */}
          {error && <div className={s["police_text"]}>{errorMessage}</div>}
        </TableCell>
        {/* Total Price */}
        <TableCell flex="1.5" align="center">
          {formatCurrency(totalPrice(item.price, item.quantity))}
        </TableCell>
        {/* Actions */}
        <TableCell flex="1.75" align="center">
          <button
            type="button"
            onClick={() => onUpdate(0)}
            style={{ padding: "1px 6px" }}
          >
            Delete
          </button>
        </TableCell>
      </TableRow>
    </div>
  );
}
