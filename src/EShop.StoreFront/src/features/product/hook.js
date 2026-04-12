import { useQuery } from "@tanstack/react-query";
import { getProduct, getVariantByOptions } from "./api";
import { useEffect, useMemo, useState } from "react";
import { formatCurrency } from "../../lib/format";
import { useDebouncedValue } from "../../hooks/useDebouncedValue";

export function useProduct(productId) {
  const [selectedOption, setSelectedOption] = useState({});

  const optionValueIds = useMemo(
    () => [...Object.values(selectedOption)].sort(),
    [selectedOption],
  );

  const fetchProduct = useQuery({
    queryKey: ["product", productId],
    queryFn: () => getProduct(productId),
    enabled: !!productId,
  });

  const fetchVariant = useQuery({
    queryKey: ["variant", productId, optionValueIds],
    queryFn: () => getVariantByOptions(productId, optionValueIds),
    enabled: !!productId && optionValueIds.length > 0,
  });

  // flatten
  const products = fetchProduct.data;
  const variants = fetchVariant.data ?? null;

  // clone + delete object
  const selectOption = (optionId, optionValueId) => {
    setSelectedOption((prev) => {
      const item = prev[optionId];
      if (item === optionValueId) {
        const clone = { ...prev };
        delete clone[optionId];
        return clone;
      }

      return {
        ...prev,
        [optionId]: optionValueId,
      };
    });
  };

  // check boolean
  const isSelected = (optionId, optionValueId) => {
    return selectedOption[optionId] === optionValueId;
  };

  // get variantId for add to cart
  const variantId = useMemo(() => {
    // product detail, get default variant id and check hasOption
    if (
      products?.data?.variantSummary?.variantId &&
      products?.data?.variantSummary?.hasOption === false
    ) {
      return products.data.variantSummary.variantId;
    }
    // if user select all options, we can get variantId from variants
    // when filter variant by options, if only one variant, we can get variantId directly
    if (variants?.data.variants?.length === 1) {
      return variants.data.variants[0].id;
    }
    return null;
  }, [products, variants]);

  useEffect(() => {
    setSelectedOption({});
  }, [productId]);

  const stock = useStock(products?.data, variants?.data);
  const price = usePrice(products?.data, variants?.data);

  return {
    products,
    variants,
    selectedOption,
    selectOption,
    isSelected,
    variantId,
    stock,
    price,
  };
}

export function useStock(products, variants) {
  const stock = useMemo(() => {
    if (variants) return variants.totalStock ?? 0;
    if (products?.variantSummary)
      return products.variantSummary.totalStock ?? 0;
    return 0;
  }, [products, variants]);
  return useDebouncedValue(stock, 500);
}

export function usePrice(products, variants) {
  const priceRange = useMemo(() => {
    const getPrice = (minPrice, maxPrice) => {
      if (!minPrice || !maxPrice) return "";
      if (minPrice === maxPrice) return formatCurrency(maxPrice);
      return formatCurrency(minPrice) + " - " + formatCurrency(maxPrice);
    };

    if (variants) {
      return getPrice(variants.minPrice, variants.maxPrice);
    }
    if (products?.variantSummary) {
      return getPrice(
        products.variantSummary.minPrice,
        products.variantSummary.maxPrice,
      );
    }
    return "";
  }, [products, variants]);

  return useDebouncedValue(priceRange, 500);
}
