import { Modal } from "@components/ui";
import clsx from "clsx";
import { ShippingAddressModal } from "./ShippingAddressModal";
import { useNavigate } from "react-router-dom";

export function ShippingAddress({
  isShowModal,
  onSetShowModal,
  data,
  onSubmit,
  status,
  onSetStatus,
}) {
  const navigate = useNavigate();

  // function
  const handleSubmit = (x) => {
    onSetShowModal(false);
    onSubmit(x);
    onSetStatus(true);
  };

  const handleCancel = () => {
    if (status) {
      onSetShowModal(false);
    } else {
      navigate("/cart");
    }
  };

  const { fullname, phoneNumber, city, zipCode, street } = data;

  return (
    <>
      {/* decoration */}
      <div className="decorators"></div>
      {/* content */}
      <div className="bg-white w-full px-4 py-6">
        <div className="flex items-center">
          <div className="mb-3">
            <div className="flex items-center gap-2">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="20"
                height="20"
                viewBox="0 0 24 24"
              >
                <path
                  fill="currentColor"
                  d="M12 2c-4.2 0-8 3.22-8 8.2c0 3.18 2.45 6.92 7.34 11.23c.38.33.95.33 1.33 0C17.55 17.12 20 13.38 20 10.2C20 5.22 16.2 2 12 2m0 10c-1.1 0-2-.9-2-2s.9-2 2-2s2 .9 2 2s-.9 2-2 2"
                />
              </svg>
              <h2 className="font-bold">Delivery Address</h2>
            </div>
          </div>
        </div>
        <div className="flex items-center">
          <div className="flex gap-3">
            <div className="w-full font-bold">
              {fullname} {phoneNumber}
            </div>
            <div className="w-full text-nowrap">
              {city && <span>{city}</span>}
              {zipCode && <span>, {zipCode}</span>}
              {street && <span>, {street}</span>}
            </div>
          </div>
          <div>
            <button
              className="px-4 text-sm text-blue-600"
              type="button"
              onClick={() => onSetShowModal(true)}
            >
              <span>Change</span>
            </button>
          </div>
        </div>
      </div>
      {/* modal */}
      <Modal open={isShowModal}>
        <ShippingAddressModal
          isShowModal={isShowModal}
          onSetShowModal={onSetShowModal}
          data={data}
          onSubmit={handleSubmit}
          onCancel={handleCancel}
        />
      </Modal>
    </>
  );
}
