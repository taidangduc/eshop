import clsx from "clsx";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { shippingAddressSchema } from "../schema";
import { Input } from "@components/ui";
import { useEffect } from "react";

export function ShippingAddressModal({
  isShowModal,
  onShowModal,
  data,
  onSubmit,
  onCancel,
}) {
  // REACT HOOK FORM
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isValid, isSubmitting },
  } = useForm({
    resolver: zodResolver(shippingAddressSchema),
    mode: "onBlur",
    reValidateMode: "onChange",
  });
  // SIDE-EFFECT
  useEffect(() => {
    if (isShowModal) {
      reset(data);
    }
  }, [isShowModal, data, reset]);

  return (
    <div className="z-1">
      <div className="bg-white">
        <div className="w-[500px] mx-auto p-6">
          <div className="text-lg">New Address</div>
          <form onSubmit={handleSubmit(onSubmit)}>
            <div className="w-full">
              {/* inputs */}
              <div className="flex flex-col mt-[6px] mb-[15px]">
                <div className="flex gap-4 mt-[6px] mb-[15px]">
                  <Input
                    type="text"
                    placeholder="Fullname"
                    maxLength="64"
                    className="flex-1"
                    {...register("fullname")}
                    error={errors.fullname?.message}
                  />
                  <Input
                    type="text"
                    placeholder="Phone Number"
                    {...register("phoneNumber")}
                    className="flex-1"
                    error={errors.phoneNumber?.message}
                  />
                </div>
                <div className="flex mt-[6px] mb-[10px]">
                  <div className="flex-1">
                    <Input
                      type="text"
                      placeholder="City"
                      maxLength="128"
                      autoComplete=""
                      {...register("city")}
                      error={errors.city?.message}
                    />
                  </div>
                  <div className="w-4"></div>
                  <div className="w-30">
                    <Input
                      type="text"
                      placeholder="zipCode"
                      maxLength="128"
                      autoComplete=""
                      {...register("zipCode")}
                      error={errors.zipCode?.message}
                    />
                  </div>
                </div>
                <div className="flex flex-col mt-[6px] mb-[15px]">
                  <Input
                    type="text"
                    placeholder="Street"
                    maxLength="128"
                    autoComplete=""
                    {...register("street")}
                    error={errors.street?.message}
                  />
                </div>
              </div>
              {/* action buttons */}
              <div className="flex items-center justify-end gap-4 mt-[6px] mb-[15px]">
                <button
                  className="text-blue-500 px-4 py-2"
                  onClick={() => onCancel()}
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  disabled={!isValid || isSubmitting}
                  className="bg-gray-900 text-white px-10 py-2"
                >
                  Submit
                </button>
              </div>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}
