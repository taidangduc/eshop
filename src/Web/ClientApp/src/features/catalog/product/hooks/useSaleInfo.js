import { useReducer, useCallback } from "react";
import { ACTION_TYPE } from "../state/actionType";
import { initialState } from "../state/initialState";
import { optionReducer } from "../state/optionReducer";

export function useSaleInfo() {
  const [state, dispatch] = useReducer(optionReducer, initialState);
  const { options, variants } = state;

  const addOption = useCallback(() => {
    dispatch({ type: ACTION_TYPE.ADD_OPTION });
  }, []);

  const addOptionValue = useCallback((optionId) => {
    dispatch({ type: ACTION_TYPE.ADD_OPTION_VALUE, payload: optionId });
  }, []);

  const deleteOption = useCallback((optionId) => {
    dispatch({ type: ACTION_TYPE.DELETE_OPTION, payload: optionId });
  }, []);

  const deleteOptionValue = useCallback((optionId, valueIndex) => {
    dispatch({
      type: ACTION_TYPE.DELETE_OPTION_VALUE,
      payload: { optionId, valueIndex },
    });
  }, []);

  const updateOption = useCallback((optionId, name) => {
    dispatch({ type: ACTION_TYPE.UPDATE_OPTION, payload: { optionId, name } });
  }, []);

  const updateOptionValue = useCallback((optionId, valueIndex, name, image) => {
    dispatch({
      type: ACTION_TYPE.UPDATE_OPTION_VALUE,
      payload: { optionId, valueIndex, name, image },
    });
  }, []);

  return {
    options,
    variants,
    addOption,
    addOptionValue,
    deleteOption,
    deleteOptionValue,
    updateOption,
    updateOptionValue,
  };
}
