import { TextField } from "@shared/components/TextField/TextField";

export function VariantBulkUpdate() {
  return (
    <div className="flex items-center">
      <div className="w-[200px]">List variant group</div>
      <div className="flex flex-grow-1 gap-4">
        <div className="flex flex-grow-1">
          <TextField placeholder="Price" />
          <TextField placeholder="Stock" />
          <TextField placeholder="SKU" />
        </div>
        <button>
          <div className="h-[38px] w-[200px] flex flex-col items-center justify-center rounded-[2px] bg-[rgb(0,0,0)] text-white">
            Apply
          </div>
        </button>
      </div>
    </div>
  );
}
