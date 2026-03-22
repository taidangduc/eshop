import fallbackImage from "@/public/default.jpg";
import { TextField } from "@/components/ui";

function DynamicCell({ opt }) {
  return opt.allowImage ? (
    <div className="flex flex-col items-center gap-1">
      <div className="aspect-square w-[60px] border border-gray-300 overflow-hidden">
        <img
          src={opt.value.imageUrl || fallbackImage}
          alt={opt.value.name}
          className="w-full h-full object-contain"
        />
      </div>
      <span>{opt.value.name}</span>
    </div>
  ) : (
    <span>{opt.value.name}</span>
  );
}

export function VariantTable({ dataTable }) {
  if (!dataTable?.length) return null;

  const columns = dataTable[0].options;

  // For each row: span count if it starts a group, 0 if covered by a rowspan above
  const rowSpans = dataTable.map((variant, i) => {
    const id = variant.options[0]?.value?.id;
    if (i > 0 && dataTable[i - 1].options[0]?.value?.id === id) return 0;
    let span = 1;
    while (
      i + span < dataTable.length &&
      dataTable[i + span].options[0]?.value?.id === id
    )
      span++;
    return span;
  });

  return (
    <table className="w-full border border-gray-300 border-collapse text-sm">
      <thead>
        <tr>
          {columns.map((col, i) => (
            <th
              key={col.id}
              className="border px-3 py-2 text-center bg-gray-50"
            >
              {col.name || `Variant group ${i + 1}`}
            </th>
          ))}
          <th className="border px-3 py-2 text-center bg-gray-50">
            <span className="text-red-500">*</span> Price
          </th>
          <th className="border px-3 py-2 text-center bg-gray-50">
            <span className="text-red-500">*</span> Stock
          </th>
          <th className="border px-3 py-2 text-center bg-gray-50">
            <span className="text-red-500">*</span> SKU
          </th>
        </tr>
      </thead>
      <tbody>
        {dataTable.map((variant, rowIndex) => (
          <tr key={variant.variantId}>
            {variant.options.map((opt, colIndex) => {
              if (colIndex === 0 && rowSpans[rowIndex] === 0) return null;
              return (
                <td
                  key={opt.id}
                  rowSpan={colIndex === 0 ? rowSpans[rowIndex] : undefined}
                  className="border px-3 py-2 align-middle text-center"
                >
                  <DynamicCell opt={opt} />
                </td>
              );
            })}
            <td className="border px-3 py-2">
              <TextField value={variant.price} />
            </td>
            <td className="border px-3 py-2">
              <TextField value={variant.stock} />
            </td>
            <td className="border px-3 py-2">
              <TextField value={variant.sku} />
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
