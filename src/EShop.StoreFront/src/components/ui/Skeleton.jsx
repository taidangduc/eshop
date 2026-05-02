import clsx from "clsx";

const styling = {
  base: "animate-skeleton bg-gray-300",
  circle: "rounded-full h-full w-full",
  square: "h-20 w-20",
  rectangle: "h-4 w-20",
  pill: "rounded-full h-auto w-full",
};

export function Circle() {
  return <div className={clsx(styling.base, styling.circle)}></div>;
}

export function Square() {
  return <div className={clsx(styling.base, styling.square)}></div>;
}

export function Rectangle({ className }) {
  return (
    <div className={clsx(styling.base, styling.rectangle, className)}></div>
  );
}

export function Pill() {
  return <div className={clsx(styling.base, styling.pill)}></div>;
}
