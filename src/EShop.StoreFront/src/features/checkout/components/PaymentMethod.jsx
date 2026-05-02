import { RadioButton, RadioGroup } from "@components/ui";

export function PaymentMethod({ items, value, onChange }) {
  return (
    <div
      className="flex items-center gap-10 py-4 px-4"
    >
      <h2 className="text-md">Payment Method</h2>
      <RadioGroup
        items={items}
        name="payment"
        value={value}
        onChange={onChange}
      />
    </div>
  );
}
