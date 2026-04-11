import { useState } from "react";

export function useCounter(val = 1) {
  const [count, setCount] = useState(val);
  const increase = () => { setCount((prev) => prev + 1) };
  const decrease = () => { setCount((prev) => Math.max(1, prev - 1)) };

  return {count, increase, decrease };
}
