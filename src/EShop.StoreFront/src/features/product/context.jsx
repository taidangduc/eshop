import { useState, useContext, createContext } from "react";

const Context = createContext(null);

export const Provider = ({ children, item }) => {
  const [image, setImage] = useState(item);

  return (
    <Context.Provider value={{ image, setImage }}>{children}</Context.Provider>
  );
};

export const useProductImage = () => {
  const ctx = useContext(Context);
  if (!ctx) {
    throw new Error("useProductImage must be used within a Provider'");
  }
  return ctx;
};
