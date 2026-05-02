import clsx from "clsx";

export function Input({
  label,
  name,
  value,
  onChange,
  placeholder,
  type = "text",
  error,
  className,
  ...rest
}) {
  return (
    <div className={className}>
      <div className="flex flex-col gap-1">
        {label && (
          <label htmlFor={name} className="font-medium">
            {label}
          </label>
        )}
        <input
          type={type}
          name={name}
          value={value}
          onChange={onChange}
          placeholder={placeholder}
          className={clsx("border px-3 py-2 mt-0 w-full")}
          {...rest}
        />
        {error && <div className="text-red-500 text-sm">{error}</div>}
      </div>
    </div>
  );
}
