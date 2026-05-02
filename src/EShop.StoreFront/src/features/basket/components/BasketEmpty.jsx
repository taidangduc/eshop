export function BasketEmpty() {
  return (
    <div className="flex flex-col items-center justify-center gap-4 py-10">
      <h2 className="text-2xl font-semibold">Your shopping cart is empty</h2>
      <a href="/" className="text-blue-600 hover:underline">
        Go to shopping
      </a>
    </div>
  );
}
