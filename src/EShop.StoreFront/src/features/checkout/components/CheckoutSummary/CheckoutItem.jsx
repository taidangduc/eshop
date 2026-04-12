import clsx from "clsx";
import s from "./CheckoutSummary.module.css";
import { formatCurrency } from "@/lib/format";
import { TableRow, TableCell } from "@/components/ui";
import { Image } from "@/components/ui";

export function CheckoutItem({ item }) {
  const totalPrice = (price, quantity) => price * quantity;

  return (
    <div className={s["checkout-item"]}>
      <TableRow>
        <TableCell
          className={clsx(s["checkout-content-table__col--main"])}
          flex="4 1 0%"
        >
          <Image
            src={item.imageUrl}
            width="40px"
            height="40px"
            className="object-contain"
            alt={item.productName}
          />
          <span className={s["checkout-item__title"]}>
            <span className={clsx("ellipsis")}>{item.title}</span>
          </span>
        </TableCell>
        <TableCell
          className={clsx(
            s["checkout-content-table__col--sub"],
            s["checkout-item-variant"],
          )}
          flex="2 1 0%"
          align="right"
        >
          {item.name && (
            <span
              className={clsx("ellipsis", s["checkout-item-variant__title"])}
            >
              Variation: {item.name}
            </span>
          )}
        </TableCell>
        <TableCell
          className={s["checkout-content-table__col--sub"]}
          flex="2 1 0%"
          align="right"
        >
          {formatCurrency(item.price)}
        </TableCell>
        <TableCell
          className={s["checkout-content-table__col--sub"]}
          flex="2 1 0%"
          align="right"
        >
          {item.quantity}
        </TableCell>
        <TableCell
          className={s["checkout-content-table__col--sub"]}
          flex="2 1 0%"
          align="right"
        >
          {formatCurrency(totalPrice(item.price, item.quantity))}
        </TableCell>
      </TableRow>
    </div>
  );
}
