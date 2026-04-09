import { formatCurrency } from "@/lib/format";
import { Link } from "react-router-dom";
import { Image } from "@/components/ui";

export function ProductCard({ product }) {
  const __image = product?.thumbnail;

  return (
    <div className="w-1/6" style={{ padding: "5px" }}>
      <div
        className="relative h-full"
        style={{ border: "1px solid rgba(0, 0, 0, 0.09)" }}
      >
        <Link to={`/product/${product.id}`} className="block h-full">
          <div className="flex flex-col h-full w-full bg-white">
            <div className="relative">
              {/* CARD IMAGE */}
              <div className="w-full aspect-square">
                <Image src={__image} className="w-full h-full object-contain" />
              </div>
              {/* CARD BADGE */}
              <div className="absolute top-0 right-0 bg-[black] px-1 product-card__discount">
                {product.percent && (
                  <span className="text-white text-xs">
                    -{product.percent}%
                  </span>
                )}
                <span className="text-white text-xs">-{99}%</span>
              </div>
            </div>
            {/* CARD CONTENT */}
            <div className="flex flex-col flex-1 p-2 justify-between">
              <div className="text-sm line-clamp-2">{product.title}</div>
              <div className="flex justify-between items-center mt-2">
                <div className="flex items-center">
                  <span className="text-base">
                    {formatCurrency(product.price)}
                  </span>
                </div>
              </div>
            </div>
          </div>
        </Link>
      </div>
    </div>
  );
}
