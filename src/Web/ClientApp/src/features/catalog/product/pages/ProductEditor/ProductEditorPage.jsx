import { BasicInfo } from "../../components/ProductInfo/BasicInfo";
import { SaleInfo } from "../../components/ProductInfo/SaleInfo";
export function ProductEditorPage() {
  return (
    <div className="flex flex-col flex-grow-1 ml-[20px] me-[20px] relative">
      <BasicInfo />
      <div className="h-[20px]"></div>
      <SaleInfo />
      {/* Toolbar */}
      <div
        className="sticky bottom-0 mt-[20px]"
        style={{ boxShadow: "0 -4px 8px -4px rgba(0, 0, 0, 0.25)" }}
      >
        <div className="flex justify-end gap-[10px] bg-white p-[20px]">
          <button className="h-[38px] w-[200px] px-[20px] rounded-[2px] border border-gray-300">
            Cancel
          </button>
          <button>
            <div className="h-[38px] w-[200px] flex flex-col items-center justify-center rounded-[2px] bg-[rgb(0,0,0)] text-white">
              Save & Hidden
            </div>
          </button>
          <button>
            <div className="h-[38px] w-[200px] flex flex-col items-center justify-center rounded-[2px] bg-[rgb(0,0,0)] text-white">
              Save & Publish
            </div>
          </button>
        </div>
      </div>
    </div>
  );
}
