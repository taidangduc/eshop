import { useSearchParams } from "react-router-dom";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { getCheckoutOrder } from "@features/checkout/api";
import { PageLoading } from "@components/ui";

export const CheckoutStatusPage = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();

  const [showLoading, setShowLoading] = useState(true);
  const [showError, setShowError] = useState(false);

  const handleRedirect = () => {
    navigate("/");
  };

  const orderNumber = searchParams.get("orderNumber") ?? "";

  useEffect(() => {
    if (!orderNumber) {
      setShowError(true);
      return;
    }

    const fetchOrderCheckout = async () => {
      try {
        const res = await getCheckoutOrder(orderNumber);
        if (res.status !== 200) {
          setShowError(true);
          return;
        }
      } catch (err) {
        setShowError(true);
        return;
      }
    };
    fetchOrderCheckout();
  }, [orderNumber]);

  // page load
  useEffect(() => {
    setShowLoading(true);
    const timer = setTimeout(() => {
      setShowLoading(false);
    }, 1000);

    return () => clearTimeout(timer);
  }, []);

  if (showLoading) {
    return <PageLoading />;
  }

  if (showError) {
    return (
      <>
        <div className="w-full min-h-screen flex items-center justify-center">
          <div className="bg-white w-[500px] flex flex-col p-6 ">
            <h2 className="text-lg font-bold mb-4">Error !!!</h2>
            <div className="mb-10">
              <span>
                We couldn't retrieve your order information. Please try again
                later.
              </span>
            </div>
            <div className="flex w-full">
              <button
                className="flex-1 border bg-gray-900 text-white py-2 mr-2"
                onClick={() => handleRedirect()}
              >
                OK, got it
              </button>
            </div>
          </div>
        </div>
      </>
    );
  }

  return (
    <>
      <div className="w-full min-h-screen flex items-center justify-center">
        <div className="bg-white w-[500px] flex flex-col p-6 ">
          <h2 className="text-lg font-bold mb-4">Processing</h2>
          <div className="mb-10">
            <span>
              Your order has been created. The order status updates
              automatically. Track your order in
              <a href="/" className="pl-1 text-blue-500 underline">
                My Orders
              </a>
              .
            </span>
          </div>
          <div className="flex w-full">
            <button
              className="flex-1 border bg-gray-900 text-white py-2 mr-2"
              onClick={() => handleRedirect()}
            >
              OK, got it
            </button>
          </div>
        </div>
      </div>
    </>
  );
};
