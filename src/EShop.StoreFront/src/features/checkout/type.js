export const PAYMENT_PROVIDERS = [
  { id: "cod", label: "Cash on Delivery", method: 1, provider: 0 },
  { id: "vnpay", label: "VnPay", method: 2, provider: 1 },
  { id: "stripe", label: "Stripe", method: 2, provider: 2 },
];

export const SHIPPING_ADDRESS_MODEL = {
  id: 0,
  name: "",
  phone: "",
  address: "",
  city: "",
  state: "",
  zip: "",
};

export const CHECKOUT_MODEL = {
  customerId: "",
  method: 1,
  provider: 0,
  street: "",
  city: "",
  zipCode: "",
};
