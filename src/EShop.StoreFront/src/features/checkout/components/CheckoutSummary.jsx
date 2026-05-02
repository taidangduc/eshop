import { formatCurrency } from "@lib/format";
import clsx from "clsx";
import {
  Table,
  TableHeader,
  TableHeaderCell,
  TableBody,
  TableRow,
  TableCell,
  Image,
} from "@components/ui";

export function CheckoutSummary({ items = [] }) {
  const totalPrice = items?.reduce(
    (sum, item) => (sum = sum + item.price * item.quantity),
    0,
  );

  return (
    <div>
      <div className="overflow-hidden">
        <Table>
          <TableHeader>
            <TableRow className="bg-white">
              <TableHeaderCell className="w-auto text-left">
                Product Ordered
              </TableHeaderCell>
              <TableHeaderCell className="w-[15%] text-center"></TableHeaderCell>
              <TableHeaderCell className="w-[15%] text-center">
                Unit Price
              </TableHeaderCell>
              <TableHeaderCell className="w-[15%] text-center">
                Amount
              </TableHeaderCell>
              <TableHeaderCell className="w-[15%] text-center">
                Item Subtotal
              </TableHeaderCell>
            </TableRow>
          </TableHeader>
          <TableBody className="bg-white">
            {items.map((item) => (
              <>
                <TableRow key={item.id} hover>
                  <TableCell>
                    <div className="flex gap-3 items-center">
                      <Image
                        src={item.imageUrl}
                        className="w-15 h-15 object-contain"
                        alt={item.productName}
                      />
                      <div>
                        <div className="font-medium">{item.title}</div>
                      </div>
                    </div>
                  </TableCell>
                  <TableCell className="text-start">
                    {item.name && (
                      <div className="text-sm text-gray-500">
                        <span>Variants:</span>
                        <div> {item.name}</div>
                      </div>
                    )}
                  </TableCell>
                  <TableCell className="text-center">
                    {formatCurrency(item.price)}
                  </TableCell>
                  <TableCell className="text-center">{item.quantity}</TableCell>
                  <TableCell className="text-center">
                    {formatCurrency(item.price * item.quantity)}
                  </TableCell>
                </TableRow>
              </>
            ))}
          </TableBody>
        </Table>
        <div
          className="flex justify-end my-4 pr-4"
          style={{ borderTop: "1px dashed rgba(0, 0, 0, 0.09)" }}
        >
          <div className="flex gap-4 items-center mt-4">
            <h3 className="text-md font-medium">
              Order Total ({items.length} Items):
            </h3>
            <div className="text-xl font-medium text-primary">
              {formatCurrency(totalPrice)}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
