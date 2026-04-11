import {
  useState,
  useContext,
  createContext,
  useEffect,
  useMemo,
  useCallback,
} from "react";

const Context = createContext(null);

export const Provider = ({ children, images }) => {
  // const [index, setIndex] = useState(0);
  // const [hovered, setHovered] = useState(null);

  const [preview, setPreview] = useState(images[0]);
  const [index, setIndex] = useState(0);
  const [sourceImage, setSourceImage] = useState(images[0]);

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
      index,
      setIndex,
    }),
    [preview, setTemporary, apply, reset, index, setIndex],
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
