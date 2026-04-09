import { useProductImage } from "../../context";
import { Image } from "@/components/ui";

export function ImagePreview() {
  const { image } = useProductImage();
  return (
    <div style={{ height: "450px", width: "450px", aspectRatio: 1 }}>
      <Image
        src={image}
        style={{ height: "100%", width: "100%", objectFit: "contain" }}
      />
    </div>
  );
}
