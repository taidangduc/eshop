import { TextField } from "@/components/ui/TextField/TextField";
import { ImageUploader } from "./ImageUploader";

export function BasicInfo() {
  return (
    <section className="bg-white rounded-[4px]">
      <div className="pb-[40px] pt-[10px] px-[20px]">
        <h2 className="text-[21px]">Basic Info</h2>
        <div className="text-[14px]">
          <div className="ml-[20px] me-[20px] mt-[20px] flex flex-col gap-[20px]">
            <div className="flex">
              <div className="flex flex-wrap w-[200px]">Image List</div>
              <ImageUploader maxImages={5} showText={true} />
            </div>
            <div className="flex">
              <div className="flex w-[200px]">Cover Image</div>
              <ImageUploader maxImages={1} showText={true} />
            </div>
            <div className="flex items-center h-[38px]">
              <div className="w-[200px]">
                <span className="text-red-500">*</span> Product Name
              </div>
              <TextField placeholder="Enter product name" />
            </div>

            <div className="flex items-center h-[38px]">
              <div className="w-[200px]">
                <span className="text-red-500">*</span> Category
              </div>
              <TextField placeholder="Enter category" />
            </div>

            <div className="flex">
              <div className="w-[200px]">
                <span className="text-red-500">*</span> Description
              </div>
              <textarea
                className="flex-grow p-2 min-h-[150px] text-[14px] border rounded-[2px] focus:outline-none"
                placeholder="Enter product description"
                maxLength={5000}
              ></textarea>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}
