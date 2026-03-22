import { TextField } from "@/components/ui/TextField/TextField";
import { ImageUploader } from "../ImageUploader";

export function VariantCreator({
  optionList,
  addOption,
  addOptionValue,
  deleteOption,
  deleteOptionValue,
  updateOption,
  updateOptionValue,
}) {
  return (
    <>
      <div>
        {optionList.map((option) => (
          <div key={option.id} className="w-full bg-gray-100 p-4 mb-2 rounded">
            <div className="flex justify-between  mb-2">
              <div className="w-[550px] bg-white">
                <TextField
                  value={option.name}
                  onChange={(e) => updateOption(option.id, e.target.value)}
                  placeholder="Option name (e.g., Color, Size)"
                />
              </div>
              <button
                onClick={() => deleteOption(option.id)}
                className="text-red-500 hover:text-red-700"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width={24}
                  height={24}
                  viewBox="0 0 24 24"
                >
                  <path
                    fill="#000"
                    d="m16.192 6.344l-4.243 4.242l-4.242-4.242l-1.414 1.414L10.535 12l-4.242 4.242l1.414 1.414l4.242-4.242l4.243 4.242l1.414-1.414L13.364 12l4.242-4.242z"
                  ></path>
                </svg>
              </button>
            </div>
            <div className="m-3 border"></div>
            <div className="grid grid-cols-2 gap-4 mb-2">
              {option.values.map((value, index) => {
                const isLastValue = index === option.values.length - 1;
                const isEmpty = !value.name || value.name.trim() === "";
                const showRemoveButton = !isLastValue || !isEmpty;

                return (
                  <div key={value.id} className="flex gap-2 items-center">
                    {/* Show image uploader only if option allows images */}
                    {option.allowImage && (
                      <div className="bg-white">
                        <ImageUploader
                          size={38}
                          maxImages={1}
                          onChange={(files) =>
                            updateOptionValue(
                              option.id,
                              index,
                              undefined,
                              files[0] ? URL.createObjectURL(files[0]) : "",
                            )
                          }
                        />
                      </div>
                    )}

                    <div className="flex-1 bg-white">
                      <TextField
                        value={value.name}
                        onChange={(e) =>
                          updateOptionValue(
                            option.id,
                            index,
                            e.target.value,
                            undefined,
                          )
                        }
                        placeholder="Value name"
                      />
                    </div>
                    {showRemoveButton && (
                      <button
                        onClick={() => deleteOptionValue(option.id, index)}
                        className="text-red-500 hover:text-red-700"
                      >
                        <svg
                          xmlns="http://www.w3.org/2000/svg"
                          width={20}
                          height={20}
                          viewBox="0 0 24 24"
                        >
                          <path
                            fill="#000"
                            d="M5 20a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V8h2V6h-4V4a2 2 0 0 0-2-2H9a2 2 0 0 0-2 2v2H3v2h2zM9 4h6v2H9zM8 8h9v12H7V8z"
                          ></path>
                          <path fill="#000" d="M9 10h2v8H9zm4 0h2v8h-2z"></path>
                        </svg>
                      </button>
                    )}
                    {!showRemoveButton && <div style={{ width: 20 }} />}
                  </div>
                );
              })}
            </div>
          </div>
        ))}
        <div className="mt-4 bg-gray-100 p-4 mb-2 rounded">
          <button onClick={() => addOption()}>
            <div className="h-[38px] w-[200px] flex flex-col gap-1 items-center justify-center border border-dashed rounded-[2px] cursor-pointer">
              + Add variant group
            </div>
          </button>
        </div>
      </div>
    </>
  );
}
