import { useProductImage } from "../context";
import { Image } from "@components/ui";

export function ImagePreview() {
  const { preview } = useProductImage();
  return (
    <div className="h-[450px] w-[450px] aspect-square">
      <Image src={preview.imageUrl} className="h-full w-full object-contain" />
    </div>
  );
}
