import { TextField } from "@shared/components";

import { VariantCreator } from "./VariantCreator";
import { VariantBulkUpdate } from "./VariantBulkUpdate";
import { VariantTable } from "./VariantTable";
import { useSaleInfo } from "../../../hooks/useSaleInfo";

export function SaleInfo() {
  const {
    options,
    variants,
    addOption,
    addOptionValue,
    deleteOption,
    deleteOptionValue,
    updateOption,
    updateOptionValue,
  } = useSaleInfo();

  // Function to convert variants and options to the format needed for the table.
  const variantList =
    variants?.length === 0
      ? []
      : variants.map((x) => ({
          variantId: x.id,
          price: x.price,
          stock: x.stock,
          sku: x.sku,
          options: options.map((opt) => {
            // Find the value for this option in the current variant.
            // Match by checking if the value exists in this option's values list,
            // so reordering or updates won't cause wrong mappings.
            // find() + some() => boolean (findFirst)
            const __value = x.optionValues?.find((o) =>
              opt.values.some((ov) => ov.id === o.id),
            );

            return {
              id: opt.id,
              name: opt.name,
              allowImage: opt.allowImage,
              value: {
                id: __value?.id ?? "",
                name: __value?.name ?? "",
                imageUrl: __value?.image ?? "",
              },
            };
          }),
        }));

  return (
    <section className="bg-white rounded-[4px]">
      <div className="pb-[40px] pt-[10px] px-[20px]">
        <h2 className="text-[21px]">Sale Info</h2>
        <div className="text-[14px]">
          <div className="ml-[20px] me-[20px] mt-[20px] flex flex-col gap-[20px]">
            <div className="flex items-start">
              <div className="flex w-[200px]">Variant Group</div>
              <div className="flex flex-col flex-grow">
                {options.length === 0 ? (
                  <>
                    <button onClick={() => addOption()}>
                      <div className="h-[38px] w-[200px] flex flex-col gap-1 items-center justify-center border border-dashed rounded-[2px] cursor-pointer">
                        + Add variant group
                      </div>
                    </button>
                  </>
                ) : (
                  <VariantCreator
                    optionList={options}
                    addOption={addOption}
                    addOptionValue={addOptionValue}
                    deleteOption={deleteOption}
                    deleteOptionValue={deleteOptionValue}
                    updateOption={updateOption}
                    updateOptionValue={updateOptionValue}
                  />
                )}
              </div>
            </div>
            {options.length === 0 ? (
              <>
                <div className="flex items-center h-[38px]">
                  <div className="w-[200px]">
                    <span className="text-red-500">*</span> Price
                  </div>
                  <div className="w-[300px]">
                    <TextField />
                  </div>
                </div>

                <div className="flex items-center h-[38px]">
                  <div className="w-[200px]">
                    <span className="text-red-500">*</span> Stock
                  </div>
                  <div className="w-[300px]">
                    <TextField />
                  </div>
                </div>
              </>
            ) : (
              <>
                <div>
                  <div className="mt-4 mb-4">
                    <VariantBulkUpdate />
                  </div>
                  <VariantTable dataTable={variantList} />
                </div>
              </>
            )}
          </div>
        </div>
      </div>
    </section>
  );
}
