import { ACTION_TYPE } from "./actionType";
import { initialState } from "./initialState";

// ---------------------------------------------------------------------------
// Helpers
// ---------------------------------------------------------------------------

export function cartesianProduct(arrays) {
  return arrays.reduce(
    (acc, array) => acc.flatMap((x) => array.map((y) => [...x, y])),
    [[]],
  );
}

export function generateVariants(optionList) {
  if (optionList.length === 0) return [];

  const validOptions = optionList
    .map((opt) => ({
      ...opt,
      values: opt.values.filter((v) => v.name?.trim()),
    }))
    .filter((opt) => opt.values.length > 0);

  if (validOptions.length === 0) return [];

  const withImage = validOptions.find((opt) => opt.allowImage);
  const valueArrays = validOptions.map((opt) => opt.values);
  const combinations = cartesianProduct(valueArrays);

  return combinations.map((combo, index) => {
    const variantName = combo.map((v) => v.name).join(" - ");
    const imageValue = withImage
      ? combo.find((v) => withImage.values.includes(v))
      : null;

    return {
      id: Date.now() + index,
      name: variantName,
      image: imageValue?.image || null,
      sku: "",
      price: "",
      stock: "",
      optionValues: combo,
    };
  });
}

export function optionReducer(state = initialState, action) {
  switch (action.type) {
    case ACTION_TYPE.ADD_OPTION: {
      // boolean, just a option has image.
      const __hasImage = state.options.some((opt) => opt.allowImage);
      const __dto = {
        id: Date.now(),
        name: "",
        values: [{ id: Date.now(), name: "", image: null }],
        allowImage: !__hasImage,
      };
      const __currentEntity = [...state.options, __dto];
      return {
        ...state,
        options: __currentEntity,
        variants: generateVariants(__currentEntity),
      };
    }

    case ACTION_TYPE.DELETE_OPTION: {
      // use filter() get list data without old value.
      const __currentEntity = state.options.filter(
        (opt) => opt.id !== action.payload,
      );
      return {
        ...state,
        options: __currentEntity,
        variants: generateVariants(__currentEntity),
      };
    }

    case ACTION_TYPE.ADD_OPTION_VALUE: {
      const __currentEntity = state.options.map((option) => {
        if (option.id !== action.payload) return option;
        return {
          ...option,
          values: [...option.values, { id: Date.now(), name: "", image: null }],
        };
      });
      return { ...state, options: __currentEntity };
    }

    case ACTION_TYPE.DELETE_OPTION_VALUE: {
      const { optionId, valueIndex } = action.payload;
      const __currentEntity = state.options.map((option) => {
        if (option.id !== optionId) return option;
        return {
          ...option,
          values: option.values.filter((_, idx) => idx !== valueIndex),
        };
      });
      return {
        ...state,
        options: __currentEntity,
        variants: generateVariants(__currentEntity),
      };
    }

    case ACTION_TYPE.UPDATE_OPTION: {
      const { optionId, name } = action.payload;
      const __currentEntity = state.options.map((option) =>
        option.id === optionId ? { ...option, name } : option,
      );
      return { ...state, options: __currentEntity };
    }

    case ACTION_TYPE.UPDATE_OPTION_VALUE: {
      const { optionId, valueIndex, name, image } = action.payload;
      const __currentEntity = state.options.map((option) => {
        if (option.id !== optionId) return option;

        const updatedValues = option.values.map((value, idx) => {
          if (idx !== valueIndex) return value;
          return {
            ...value,
            ...(name !== undefined && { name }),
            ...(image !== undefined && { image }),
          };
        });

        // Auto-add trailing empty value when typing in the last slot
        if (name !== undefined) {
          const isLastValue = valueIndex === option.values.length - 1;
          const hasContent = name.trim() !== "";
          if (isLastValue && hasContent) {
            updatedValues.push({ id: Date.now() + 1, name: "", image: null });
          }
        }

        return { ...option, values: updatedValues };
      });
      return {
        ...state,
        options: __currentEntity,
        variants:
          name !== undefined
            ? generateVariants(__currentEntity)
            : state.variants,
      };
    }

    default:
      return state;
  }
}
