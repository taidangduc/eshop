import { useProductImage } from "../../context";
import s from "./index.module.css";
import clsx from "clsx";
import { Checkbox } from "@/components/ui";
import { Image } from "@/components/ui";

export function VariantSelector({ options, selectedOption, onChange }) {
  const { setTemporary, reset } = useProductImage();

  return (
    <div>
      {options &&
        options.map((o, index) => {
          const __isMax = o.values.length > 20;
          return (
            <section key={o.id} className={s["option-section"]}>
              <h2 className={s["option__title"]}>{o.name}</h2>
              <div
                className={clsx(
                  s["option-area"],
                  __isMax && s["option-area-view"],
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
                          !ov.imageUrl
                            ? s["option-value-no-image"]
                            : s["option-value-with-image"],
                          s["option-value__button"],
                        )}
                      >
                        {ov.imageUrl && (
                          <div className={s["option-value__image"]}>
                            <Image src={ov.imageUrl} />
                          </div>
                        )}
                        <span className={s["option-value__title"]}>
                          {ov.name}
                        </span>
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
