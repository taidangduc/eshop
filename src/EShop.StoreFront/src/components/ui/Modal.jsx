// ref: https://www.w3schools.com/react/react_portals.asp
// ref: https://react.dev/reference/react-dom/createPortal
import { createPortal } from "react-dom";

export function Modal({ open, onClose, children }) {
  const currentRoot = document.getElementById("modal");
  if (!currentRoot) return null;
  if (!open) return null;

  return createPortal(
    <div
      tabIndex="0"
      className="fixed bg-black/25 inset-0 flex items-center justify-center z-[999]"
      onClick={onClose}
    >
      <div
        role="dialog"
        className="relative block w-auto text-md bg-transparent"
        onClick={(e) => e.stopPropagation()}
      >
        {children}
      </div>
    </div>,
    currentRoot,
  );
}
