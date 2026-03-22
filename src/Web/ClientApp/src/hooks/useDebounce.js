import { useState, useEffect } from "react";

export function useDebounce(value, deltaTime = 500) {
  const [valueObject, setValueObject] = useState(value);

  useEffect(() => {
    const timer = setTimeout(() => {
      setValueObject(value);
    }, deltaTime);
    return () => clearTimeout(timer);
  }, [value, deltaTime]);

  return valueObject;
}
