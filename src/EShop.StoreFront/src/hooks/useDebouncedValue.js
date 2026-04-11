import { useState, useEffect } from "react";

export function useDebouncedValue(val, delayMs) {
  const [prev, setPrev] = useState(val);

  useEffect(() => {
    const timeout = setTimeout(() => setPrev(val), delayMs);
    return () => clearTimeout(timeout);
  }, [val, delayMs]);

  return prev;
}
