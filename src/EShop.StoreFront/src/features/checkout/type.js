export const PAYMENT_PROVIDERS = [
  { id: "cod", label: "Cash on Delivery", method: 1, provider: 0 },
  { id: "stripe", label: "Stripe", method: 2, provider: 2 },
];

export const SHIPPING_ADDRESS_MODEL = {
  fullname: "",
  phoneNumber: "",
  city: "",
  zipCode: "",
  street: "",
};

export const PAYMENT_MODEL = {
  method: 1,
  provider: 0,
};

export const CHECKOUT_MODEL = {
  customerId: "",
  method: 1,
  provider: 0,
  street: "",
  city: "",
  zipCode: "",
};
