import {
  useState,
  useContext,
  createContext,
  useMemo,
  useCallback,
} from "react";

const Context = createContext(null);

export const Provider = ({ children, images }) => {
  const [preview, setPreview] = useState(images[0]);
  const [sourceImage, setSourceImage] = useState(images[0]);

  // in this project have 2 source of image
  // 1. image from product image
  // 2. image from prouct option value

  const setTemporary = useCallback((img) => {
    setPreview(img);
  }, []);

  const apply = useCallback((img) => {
    setSourceImage(img);
    setPreview(img);
  }, []);

  const reset = useCallback(() => {
    setPreview(sourceImage);
  }, [sourceImage]);

  const ctx = useMemo(
    () => ({
      preview,
      setTemporary,
      apply,
      reset,
    }),
    [preview, setTemporary, apply, reset],
  );

  return <Context.Provider value={ctx}>{children}</Context.Provider>;
};

export const useProductImage = () => {
  const ctx = useContext(Context);
  if (!ctx) {
    throw new Error("useProductImage must be used within a Provider'");
  }
  return ctx;
};
