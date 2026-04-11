import { useProductImage } from "../../context";
import { Image } from "@/components/ui";

export function ImagePreview() {
  const { preview } = useProductImage();
  return (
    <div style={{ height: "450px", width: "450px", aspectRatio: 1 }}>
      <Image
        src={preview.imageUrl}
        style={{ height: "100%", width: "100%", objectFit: "contain" }}
      />
    </div>
  );
}
