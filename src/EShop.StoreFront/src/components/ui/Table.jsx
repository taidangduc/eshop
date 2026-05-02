import clsx from "clsx";

export function Table({ children, className, ...props }) {
  return (
    <table
      className={clsx("w-full border-collapse text-sm rounded-none", className)}
      {...props}
    >
      {children}
    </table>
  );
}

export function TableHeader({ children, className, ...props }) {
  return (
    <thead className={clsx("bg-gray-100", className)} {...props}>
      {children}
    </thead>
  );
}

export function TableBody({ children, className, ...props }) {
  return (
    <tbody className={clsx(className)} {...props}>
      {children}
    </tbody>
  );
}

export function TableRow({ children, className, hover, ...props }) {
  return (
    <tr
      className={clsx(hover && "hover:bg-gray-200 transition", className)}
      {...props}
    >
      {children}
    </tr>
  );
}

export function TableHeaderCell({ children, className, align = "", ...props }) {
  return (
    <th
      className={clsx(
        `py-3 px-4 font-medium text-gray-700`,
        className,
        align && `text-${align}`,
      )}
      {...props}
    >
      {children}
    </th>
  );
}

export function TableCell({ children, className, align = "", ...props }) {
  return (
    <td
      className={clsx(`py-3 px-4`, className, align && `text-${align}`)}
      {...props}
    >
      {children}
    </td>
  );
}
