import clsx from "clsx";

export function QuantitySelector({
  stock,
  count,
  onShow,
  onIncrease,
  onDecrease,
}) {
  const handleStockStatus = (stock) => {
    if (!stock) return "INSTOCK"; // for test
    if (stock === 0) return "OUT OF STOCK";
    if (stock) return `${stock} pieces stock`;
  };

  return (
    <section className="flex items-center mt-5">
      <h2 className="w-[100px] mr-2">Quantity</h2>
      <div className="flex items-center">
        <div className="mr-5">
          <div className="flex items-center justify-center border border-gray-300 w-fit mx-auto bg-white">
            <button
              className={clsx(
                "border-r border-gray-300 h-8 px-1 cursor-pointer",
                onShow && count > 1 ? "text-gray-900" : "text-gray-300",
              )}
              onClick={() => onDecrease()}
              disabled={count <= 1}
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
            <span className="w-10 text-center">{count}</span>
            <button
              className={clsx(
                "border-l border-gray-300 h-8 px-1 cursor-pointer",
                onShow ? "text-gray-900" : "text-gray-300",
              )}
              onClick={() => onIncrease()}
              disabled={!onShow || count + 1 > stock}
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
        </div>
        <div>{handleStockStatus(stock)}</div>
      </div>
    </section>
  );
}
