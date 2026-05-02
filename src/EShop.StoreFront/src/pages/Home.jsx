import { ProductList } from "@features/product";
import { useState } from "react";

export function HomePage() {
  return (
    <>
      <div className="my-5 w-[1200px] mx-auto">
        <ProductList />
      </div>
    </>
  );
}
