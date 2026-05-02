import clsx from "clsx";
export function RadioButton({ label, name, value, checked, onChange }) {
  return (
    <label
      className={clsx(
        "relative inline-flex items-center cursor-pointer border py-2 px-4 hover:border-gray-600 bg-white",
        {
          "border-gray-900": checked,
          "border-gray-300": !checked,
        },
      )}
      aria-checked={checked}
      role="radio"
    >
      <input
        type="radio"
        name={name}
        value={value}
        checked={checked}
        onChange={() => onChange(value)}
        className="form-radio hidden"
      />
      <span className="text-sm">{label}</span>
      {checked && <span className="checkmarker"></span>}
    </label>
  );
}

export function RadioGroup({ items, name, value, onChange, className }) {
  return (
    <div className={`flex gap-2 ${className}`}>
      {items.map((item) => (
        <RadioButton
          key={item.value}
          label={item.label}
          name={name}
          value={item.value}
          checked={value === item.value}
          onChange={onChange}
        />
      ))}
    </div>
  );
}
