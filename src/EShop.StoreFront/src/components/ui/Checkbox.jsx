import clsx from "clsx";
export function Checkbox({
  name,
  value,
  checked,
  onChange,
  className,
  children,
  ...props
}) {
  return (
    <label
      className={clsx(
        "relative inline-flex items-center cursor-pointer border py-2 px-4 hover:border-gray-600 bg-white",
        {
          "border-gray-900": checked,
          "border-gray-300": !checked,
        },
        className,
      )}
      role="checkbox"
      aria-checked={checked}
      {...props}
    >
      <input
        type="checkbox"
        name={name}
        value={value}
        checked={checked}
        onChange={(e) => onChange(e.target.checked, value)}
        className="form-checkbox hidden"
      />
      {checked && <span className="checkmarker"></span>}
      <div>{children}</div>
    </label>
  );
}
