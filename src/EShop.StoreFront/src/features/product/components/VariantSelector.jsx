import { useProductImage } from "../context";
import clsx from "clsx";
import { Checkbox } from "@components/ui";
import { Image } from "@components/ui";

export function VariantSelector({ options, selectedOption, onChange }) {
  const { setTemporary, reset } = useProductImage();

  return (
    <div>
      {options &&
        options.map((o, index) => {
          const __isMax = o.values.length > 20;
          return (
            <section key={o.id} className="flex items-baseline mb-5">
              <h2 className="w-[100px] block mr-[5px]">{o.name}</h2>
              <div
                className={clsx(
                  "flex flex-wrap items-center",
                  __isMax && "max-h-[192px] overflow-y-hidden",
                )}
              >
                {o.values.map((ov) => {
                  const __selected = selectedOption[o.id] === ov.id;

                  return (
                    <div key={ov.id}>
                      <Checkbox
                        checked={__selected}
                        onChange={() => onChange(o.id, ov.id)}
                        onMouseEnter={() => {
                          if (ov.imageUrl) setTemporary(ov);
                        }}
                        onMouseLeave={() => {
                          if (ov.imageUrl) reset();
                        }}
                        className={clsx(
                          "relative cursor-pointer mx-1",
                          !ov.imageUrl
                            ? "py-3 justify-center min-w-[80px]"
                            : "py-3 pl-10",
                        )}
                      >
                        {ov.imageUrl && (
                          <div className="absolute left-[5px] top-[5px] w-[24px] h-[24px]">
                            <Image
                              src={ov.imageUrl}
                              className="object-contain "
                            />
                          </div>
                        )}
                        <span>{ov.name}</span>
                      </Checkbox>
                    </div>
                  );
                })}
              </div>
            </section>
          );
        })}
    </div>
  );
}
