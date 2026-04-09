import { ProductList } from "@features/product/components";
import s from "./index.module.css";
import clsx from "clsx";

export function HomePage() {
  return (
    <div>
      <div className={s["layout-main"]}>
        <div className={clsx(s["suggest"], "container-wrapper")}>
          {/* <h1 className={s["suggest__label"]}>DAILY DISCOVER</h1> */}
          <hr className={s["suggest__divider"]} />
        </div>
        <div className={s["layout-section"]}>
          <ProductList />
        </div>
      </div>
    </div>
  );
}
