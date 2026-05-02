import { TableCell, TableRow, Image, Square, Rectangle } from "@components/ui";
import { Link } from "react-router-dom";
import { formatCurrency } from "@lib/format";

export function BasketItem({ item, onUpdate, isLoading, error, errorMessage }) {
  const totalPrice = (price, quantity) => price * quantity;

  if (isLoading) {
    return (
      <>
        <TableRow hover className="border border-gray-300">
          <TableCell>
            <div className="flex gap-3 items-center">
              <Square className="w-20 h-20" />
              <div>
                <Rectangle className="w-40" />
              </div>
            </div>
          </TableCell>
          <TableCell align="start">
            <Rectangle className="mb-1" />
            <Rectangle />
          </TableCell>
          <TableCell>
            <Rectangle className="h-4 w-20 mx-auto" />
          </TableCell>
          <TableCell>
            <Rectangle className="h-8 w-30 mx-auto" />
          </TableCell>
          <TableCell>
            <Rectangle className="h-4 w-20 mx-auto" />
          </TableCell>
          <TableCell align="center">
            <button className="text-red-600" disabled>
              Delete
            </button>
          </TableCell>
        </TableRow>
      </>
    );
  }

  return (
    <>
      <TableRow key={item.id} hover className="border border-gray-300 w-full">
        <TableCell>
          <div className="flex gap-3 items-center">
            <Image
              src={item.imageUrl}
              alt={item.title}
              className="w-20 h-20 object-contain"
            />
            <div className="font-medium">{item.title}</div>
          </div>
        </TableCell>
        <TableCell>
          {item.name && (
            <div className="text-sm text-gray-500">
              <span>Variants:</span>
              <div> {item.name}</div>
            </div>
          )}
        </TableCell>
        <TableCell align="center">{formatCurrency(item.price)}</TableCell>
        <TableCell align="center">
          <div className="relative">
            <div className="flex items-center justify-center border border-gray-300 w-fit mx-auto">
              <button
                className="h-8 px-1 border-r border-gray-300"
                onClick={() => onUpdate(item.quantity - 1)}
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="24"
                  height="24"
                  viewBox="0 0 24 24"
                >
                  <path
                    fill="currentColor"
                    d="M18 12.998H6a1 1 0 0 1 0-2h12a1 1 0 0 1 0 2"
                  />
                </svg>
              </button>
              <span className="w-10">{item.quantity}</span>
              <button
                className="h-8 px-1 border-l border-gray-300"
                onClick={() => onUpdate(item.quantity + 1)}
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="24"
                  height="24"
                  viewBox="0 0 24 24"
                >
                  <path
                    fill="currentColor"
                    d="M19 12.998h-6v6h-2v-6H5v-2h6v-6h2v6h6z"
                  />
                </svg>
              </button>
            </div>
            {error && (
              <div className="absolute text-[12px] text-red-600 w-full left-0 mt-1">
                {errorMessage}
              </div>
            )}
          </div>
        </TableCell>
        <TableCell align="center">
          {formatCurrency(totalPrice(item.price, item.quantity))}
        </TableCell>
        <TableCell align="center">
          <button
            className="text-red-600 cursor-pointer"
            onClick={() => onUpdate(0)}
          >
            Delete
          </button>
        </TableCell>
      </TableRow>
    </>
  );
}
